using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Services
{
    public class PaymentAdvanceService: IPaymentAdvanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IApprovalBoardActiveLevelService _approvalBoardActiveLevelService;
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _sqlConnection;
        private readonly PayrollContext _payrollContext;

        public PaymentAdvanceService(IUnitOfWork unitOfWork, IEmployeeApprovalConfigService employeeApprovalConfigService, IApprovalBoardService approvalBoardService, IApprovalBoardActiveLevelService approvalBoardActiveLevelService, INotificationService notificationService, IConfiguration configuration, PayrollContext payrollContext)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _approvalBoardService = approvalBoardService;
            _approvalBoardActiveLevelService = approvalBoardActiveLevelService;
            _notificationService = notificationService;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:PRServerConnection"]);
            _payrollContext = payrollContext;
        }

        public async Task<BaseResponse> Create(PaymentAdvance model)
        {
            //check if he has applied during the year
            var check = await GetAll(x => x.CreatedDate.Year == DateTime.Now.Year);
            var checkMonth = (await GetAll(x => x.TargetDate.Month == model.TargetDate.Month && x.TargetDate.Year == model.TargetDate.Year)).FirstOrDefault();
            if (checkMonth != null)
            {
                return new BaseResponse() { Status = false, Message = ResponseMessage.PaymentAdvanceExist };
            }
            if (check.Count() <= 3)
            {
                _unitOfWork.GetRepository<PaymentAdvance>().Insert(model);
                //await _unitOfWork.SaveChangesAsync();

                //submit for approval
                var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("payment advance"));
                var approvalProcessor = await _employeeApprovalConfigService.GetBy(x => x.EmployeeId == model.EmployeeId && x.ApprovalLevel == Level.FirstLevel && x.ApprovalWorkItemId == approvalWorkItem.Id);
                try
                {
                    var enlistBoard = new ApprovalBoard()
                    {
                        EmployeeId = model.EmployeeId,
                        ApprovalLevel = Level.FirstLevel,
                        Emp_No = model.Emp_No,
                        ApprovalWorkItemId = approvalWorkItem.Id,
                        ApprovalProcessorId = approvalProcessor.ProcessorIId.Value,
                        ApprovalProcessor = approvalProcessor.Processor,
                        ServiceId = model.Id,
                        Status = ApprovalStatus.Pending,
                        CreatedDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        CreatedBy = model.Emp_No
                    };
                    await _approvalBoardService.Create(enlistBoard);
                    await _approvalBoardActiveLevelService.CreateOrUpdate(approvalWorkItem.Id, model.Id, Level.FirstLevel);
                    await _notificationService.CreateNotification(NotificationAction.AdvanceCreateTitle, NotificationAction.AdvanceCreateMessage, model.EmployeeId, false, false);
                    if (approvalProcessor != null)
                    {
                        await _notificationService.CreateNotification(NotificationAction.NewApprovalCreateTitle, NotificationAction.ApprovalCreateMessage, approvalProcessor.ProcessorIId.Value, false, false);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }


                var checkTracking = await _unitOfWork.GetRepository<PaymentAdvanceTrack>().GetFirstOrDefaultAsync(predicate: x => x.Year == model.TargetDate.Year && x.EmployeeId == model.EmployeeId);
                if(checkTracking != null)
                {
                    checkTracking.Count += 1;
                    _unitOfWork.GetRepository<PaymentAdvanceTrack>().Update(checkTracking);
                }
                else
                {
                    var tracking = new PaymentAdvanceTrack() { Count = 1, EmployeeId = model.EmployeeId, Emp_No = model.Emp_No, Id = Guid.NewGuid(), CreatedDate = DateTime.Now, Year = model.TargetDate.Year};
                    _unitOfWork.GetRepository<PaymentAdvanceTrack>().Insert(tracking);
                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.MaximumPaymentAdvanceApplied };
        }

        public async Task<IEnumerable<PaymentAdvance>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()), "Employee");
            return data;
        }

        public async Task<PaymentAdvance> GetById(Guid id)
        {
            var data = await _unitOfWork.GetRepository<PaymentAdvance>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            return data;
        }

        public async Task<IEnumerable<PaymentAdvance>> GetByEmployee(Guid employeeId)
        {
            var data = await GetAll(x => x.EmployeeId == employeeId, "Employee");
            return data;
        }

        public async Task<IEnumerable<PaymentAdvance>> GetAll(Expression<Func<PaymentAdvance, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<PaymentAdvance>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<PaymentAdvanceResponseModel> CheckEligibility(string emp_No)
        {
            var applicationCount = _unitOfWork.GetRepository<PaymentAdvanceTrack>().Count(predicate: x => x.Emp_No.ToLower() == emp_No.ToLower() && x.CreatedDate.Year == DateTime.Now.Year);

            decimal maximumAmount = 0;

            //var resource = _payrollContextSqlQuery<decimal>.FromSql($"Select dbo.ESSBasic(${emp_No})");

            using (_sqlConnection)
            {
                _sqlConnection.Open();

                SqlCommand cmd = new SqlCommand($"Select dbo.ESSBasic(@EmpNo)", _sqlConnection);
                cmd.Parameters.Add(new SqlParameter("@EmpNo", $"{emp_No}"));
                cmd.CommandType = CommandType.Text;
                
                maximumAmount = cmd.ExecuteNonQuery();
            }

            return new PaymentAdvanceResponseModel() { ApplicationCount = applicationCount, MaximumAllowedAmount = maximumAmount };
        }
    }
}
