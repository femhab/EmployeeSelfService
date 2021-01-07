using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class PaymentAdvanceService: IPaymentAdvanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IApprovalBoardActiveLevelService _approvalBoardActiveLevelService;
        private readonly INotificationService _notificationService;
        private readonly SqlConnection _sqlConnection;

        public PaymentAdvanceService(IUnitOfWork unitOfWork, IEmployeeApprovalConfigService employeeApprovalConfigService, IApprovalBoardService approvalBoardService, IApprovalBoardActiveLevelService approvalBoardActiveLevelService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _approvalBoardService = approvalBoardService;
            _approvalBoardActiveLevelService = approvalBoardActiveLevelService;
            _notificationService = notificationService;
            _sqlConnection = new SqlConnection(PayrollDbConfig.ConnectionStringUrl);
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

            using (_sqlConnection)
            {
                _sqlConnection.Open();

                SqlCommand cmd = new SqlCommand("Select dbo.ESSBasic(@EmpNo)", _sqlConnection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@EmpNo", emp_No));
                
                maximumAmount = cmd.ExecuteNonQuery();
            }

            return new PaymentAdvanceResponseModel() { ApplicationCount = applicationCount, MaximumAllowedAmount = maximumAmount };
        }
    }
}
