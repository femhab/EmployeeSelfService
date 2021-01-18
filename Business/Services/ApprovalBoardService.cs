using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class ApprovalBoardService: IApprovalBoardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IApprovalBoardActiveLevelService _approvalBoardActiveLevelService;
        private readonly SqlConnection _sqlConnection;
        private readonly SqlConnection _prSqlConnection;
        private readonly IConfiguration _configuration;


        public ApprovalBoardService(IUnitOfWork unitOfWork, IEmployeeApprovalConfigService employeeApprovalConfigService, IApprovalBoardActiveLevelService approvalBoardActiveLevelService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _approvalBoardActiveLevelService = approvalBoardActiveLevelService;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
            _prSqlConnection = new SqlConnection(_configuration["ConnectionStrings:PRServerConnection"]);
        }

        public async Task<BaseResponse> ApprovalAction(Guid ProcessorId, ApprovalStatus status, Level approvalLevel, Guid serviceId, Guid approvalWorkItemId)
        {
            var data = await _unitOfWork.GetRepository<ApprovalBoard>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalWorkItemId == approvalWorkItemId && x.ServiceId == serviceId && x.ApprovalLevel == approvalLevel);

            if(data != null && data.ApprovalProcessorId == ProcessorId)
            {
                if (data.Status != status)
                {
                    data.Status = status;
                    data.UpdatedDate = DateTime.Now;
                }
                else
                {
                    return new BaseResponse() { Status = false, Message = ResponseMessage.AlreadyApproved };
                }

                _unitOfWork.GetRepository<ApprovalBoard>().Update(data);

                if (status == ApprovalStatus.Approved)
                {
                    //look for new approver for next level and assign the neccessary
                   var newApprovalLevel = approvalLevel + 1;
                    var approvalProcessor = await _employeeApprovalConfigService.GetBy(x => x.EmployeeId == data.EmployeeId && x.ApprovalLevel == newApprovalLevel && x.ApprovalWorkItemId == data.ApprovalWorkItemId);

                    var enlistBoard = new ApprovalBoard()
                    {
                        EmployeeId = data.EmployeeId,
                        ApprovalLevel = (approvalProcessor != null) ? newApprovalLevel: Level.HR,
                        Emp_No = data.Emp_No,
                        ApprovalWorkItemId = data.ApprovalWorkItemId,
                        ApprovalProcessorId = (approvalProcessor != null) ? approvalProcessor.ProcessorIId.Value: Guid.Empty,
                        ApprovalProcessor = (approvalProcessor != null) ? approvalProcessor.Processor : null,
                        ServiceId = data.ServiceId,
                        Status = ApprovalStatus.Pending,
                        CreatedDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        CreatedBy = data.Emp_No
                    };

                    await Create(enlistBoard);
                    try
                    {
                        await _approvalBoardActiveLevelService.CreateOrUpdate(data.ApprovalWorkItemId, data.ServiceId, (approvalProcessor != null) ? newApprovalLevel : Level.HR);
                        if(enlistBoard.ApprovalLevel == Level.HR)
                        {
                            var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Id == data.ApprovalWorkItemId);
                            if (approvalWorkItem.Name.ToLower() == "leave")
                            {
                                var leave = await _unitOfWork.GetRepository<Leave>().GetFirstOrDefaultAsync(predicate: x => x.Id == data.ServiceId, null, c => c.Include(i => i.LeaveType));
                                await WriteLeaveToHR(leave);
                                //
                            }
                            else if (approvalWorkItem.Name.ToLower() == "recall")
                            {
                                var recall = await _unitOfWork.GetRepository<LeaveRecall>().GetFirstOrDefaultAsync(predicate: x => x.Id == data.ServiceId);
                                //await WriteLeaveToHR(leave);
                                
                            }
                            else if (approvalWorkItem.Name.ToLower() == "training")
                            {
                                var training = await _unitOfWork.GetRepository<Training>().GetFirstOrDefaultAsync(predicate: x => x.Id == data.ServiceId);
                                var trainingCalender = await _unitOfWork.GetRepository<TrainingCalender>().GetFirstOrDefaultAsync(predicate: x => x.Topic.Title.ToLower() == training.TrainingTopic.ToLower());
                                await WriteTrainingToHR(training, (trainingCalender == null) ? 0: trainingCalender.HRTrainingCalenderID);
                            }
                            else if(approvalWorkItem.Name.ToLower() == "loan")
                            {
                                var loan = await _unitOfWork.GetRepository<Loan>().GetFirstOrDefaultAsync(predicate: x => x.Id == data.ServiceId);
                                await WriteLoanToHR(loan);
                            }
                            else if (approvalWorkItem.Name.ToLower() == "transfer")
                            {
                                var transfer = await _unitOfWork.GetRepository<AppliedTransfer>().GetFirstOrDefaultAsync(predicate: x => x.Id == data.ServiceId, include: c => c.Include(i => i.Department).Include(i => i.Division).Include(i => i.Section).Include(i => i.Employee).Include(i => i.Employee.Department).Include(i => i.Employee.Section).Include(i => i.Employee.Division));
                                await WriteTransferToHR(transfer);
                            }
                            else if (approvalWorkItem.Name.ToLower() == "payment advance")
                            {
                                var advance = await _unitOfWork.GetRepository<PaymentAdvance>().GetFirstOrDefaultAsync(predicate: x => x.Id == data.ServiceId);
                                await WriteAdvanceToHR(advance);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    
                }

                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.ApprovedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Create(ApprovalBoard model)
        {
            var check = await _unitOfWork.GetRepository<ApprovalBoard>().GetFirstOrDefaultAsync(predicate: x => x.ServiceId == model.ServiceId && x.ApprovalWorkItemId == model.ApprovalWorkItemId && x.EmployeeId == model.EmployeeId && x.ApprovalLevel == model.ApprovalLevel);
            if (check == null)
            {
                try
                {
                    _unitOfWork.GetRepository<ApprovalBoard>().Insert(model);
                    await _unitOfWork.SaveChangesAsync();
                    return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.RecordExist };
        }

        public async  Task<IEnumerable<ApprovalBoard>> GetApprovalUpdate(Guid serviceId, Guid approvalWorkItemId)
        {
            var model = await GetAll(x => x.ServiceId == serviceId && x.ApprovalWorkItemId == approvalWorkItemId, "Employee,ApprovalWorkItem");
            return model;
        }
         
        public async Task<IEnumerable<ApprovalBoard>> GetByProcessor(Guid processorId)
        {
            var model = await GetAll(x => x.ApprovalProcessorId == processorId && x.Status == ApprovalStatus.Pending, "Employee,ApprovalWorkItem");
            return model;
        }

        public async Task<IEnumerable<ApprovalBoard>> GetAll(Expression<Func<ApprovalBoard, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<ApprovalBoard>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<IPagedList<ApprovalBoard>> GetPagedList(Expression<Func<ApprovalBoard, bool>> predicate = null, Func<IQueryable<ApprovalBoard>, IIncludableQueryable<ApprovalBoard, object>> include = null, int pageIndex = 0, int pageSize = 20)
        {

            var model = await _unitOfWork.GetRepository<ApprovalBoard>().GetPagedListAsync(predicate, source => source.OrderBy(c => c.CreatedDate), include: e => e.Include(i => i.Employee).Include(x => x.ApprovalWorkItem).Include(i => i.ApprovalProcessor), pageIndex: pageIndex, pageSize: pageSize);
            return model;
        }

        public async Task<ApprovalBoard> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<ApprovalBoard>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }

        public async Task<ApprovalBoard> GetUnsignedAppraisal(Guid serviceId)
        {
            var model = await _unitOfWork.GetRepository<ApprovalBoard>().GetFirstOrDefaultAsync(predicate: c => c.ServiceId == serviceId && c.SignOff == false);
            return model;
        }

        public async Task<BaseResponse> SignOffAppraisal(Guid appraisalId)
        {
            var model = await GetUnsignedAppraisal(appraisalId);
            model.SignOff = true;
            if(model != null)
            {
                _unitOfWork.GetRepository<ApprovalBoard>().Update(model);
                await _unitOfWork.SaveChangesAsync();
            }
            return new BaseResponse() { Status = true, Message = ResponseMessage.SignOffSuccessful };
        }

        public async Task WriteLeaveToHR(Leave leave)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = _sqlConnection;
                    cmd.CommandType = CommandType.Text;
                    var leaveType = "";
                    if(leave.LeaveType.Class == LeaveClass.Annual)
                    {
                        leaveType = "ANU";
                    }
                    if (leave.LeaveType.Class == LeaveClass.Casual)
                    {
                        leaveType = "CAS";
                    }
                    if (leave.LeaveType.Class == LeaveClass.Exam)
                    {
                        leaveType = "EXM";
                    }
                    if (leave.LeaveType.Class == LeaveClass.Maternity)
                    {
                        leaveType = "MAT";
                    }
                    if (leave.LeaveType.Class == LeaveClass.Sick)
                    {
                        leaveType = "SIK";
                    }
                    if (leave.LeaveType.Class == LeaveClass.Study)
                    {
                        leaveType = "STD";
                    }

                    cmd.CommandText = @"INSERT INTO HREmpLeaveAct(Emp_No,LeaveCode,fromm,too,leave_days,resum_date,Emp_Comment,FromDate,ToDate,Status,HRStatus,DateOfApplication,LeaveAllowance,Insertusername,InsertTransacDate,InsertTransacType) 
                                VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14,@param15,@param16)";

                    cmd.Parameters.AddWithValue("@param1", leave.Emp_No);
                    cmd.Parameters.AddWithValue("@param2", leaveType);
                    cmd.Parameters.AddWithValue("@param3", leave.DateFrom);
                    cmd.Parameters.AddWithValue("@param4", leave.DateTo);
                    cmd.Parameters.AddWithValue("@param5", leave.NoOfDays);
                    cmd.Parameters.AddWithValue("@param6", leave.ResumptionDate);
                    cmd.Parameters.AddWithValue("@param7", DBNull.Value);
                    cmd.Parameters.AddWithValue("@param8", leave.DateFrom.ToString());
                    cmd.Parameters.AddWithValue("@param9", leave.DateTo.ToString());
                    cmd.Parameters.AddWithValue("@param10", 3);
                    cmd.Parameters.AddWithValue("@param11", 0);
                    cmd.Parameters.AddWithValue("@param12", leave.CreatedDate);
                    cmd.Parameters.AddWithValue("@param13", leave.IsAllowanceRequested);
                    cmd.Parameters.AddWithValue("@param14", "ESS");
                    cmd.Parameters.AddWithValue("@param15", DateTime.Now);
                    cmd.Parameters.AddWithValue("@param16", "Insert");

                    _sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public async Task WriteTrainingToHR(Training training, int trainingCalenderId)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = _sqlConnection;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = @"INSERT INTO EmployeeTrainings(New_TrainingCalendarID,AlternateTopic,AltStartDate,AltEndDate,HRStatus,Emp_No, MgrStatus) 
                                VALUES(@param1,@param2,@param3,@param4,@param5,@param6, @param7)";
                    //cmd.Parameters.AddWithValue("@param1", 0);
                    cmd.Parameters.AddWithValue("@param1", trainingCalenderId);
                    cmd.Parameters.AddWithValue("@param2", training.TrainingTopic);
                    cmd.Parameters.AddWithValue("@param3", training.StartDate);
                    cmd.Parameters.AddWithValue("@param4", training.EndDate);
                    cmd.Parameters.AddWithValue("@param5", 0);
                    cmd.Parameters.AddWithValue("@param6", training.Emp_No);
                    cmd.Parameters.AddWithValue("@param7", 3);

                    _sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public async Task WriteLoanToHR(Loan loan)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = _prSqlConnection;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = @"INSERT INTO PRLNAPRV(emp_no,start_date,end_date,amt_rqst,amt_aprv,no_of_pay,mon_ded,hrstatus,FromDate,ToDate,ValueDate,Insertusername,InsertTransacDate,InsertTransacType, status) 
                                VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14, @param15)"; 
                    cmd.Parameters.AddWithValue("@param1", loan.Emp_No);
                    cmd.Parameters.AddWithValue("@param2", loan.StartDate);
                    cmd.Parameters.AddWithValue("@param3", loan.EndDate);
                    cmd.Parameters.AddWithValue("@param4", loan.AmountRequested);
                    cmd.Parameters.AddWithValue("@param5", loan.AmountApproved);
                    cmd.Parameters.AddWithValue("@param6", loan.NoOfInstallment);
                    cmd.Parameters.AddWithValue("@param7", loan.InstallmentAmount);
                    cmd.Parameters.AddWithValue("@param8", 0);
                    cmd.Parameters.AddWithValue("@param9", loan.StartDate.ToString());
                    cmd.Parameters.AddWithValue("@param10", loan.EndDate.ToString());
                    cmd.Parameters.AddWithValue("@param11", loan.CreatedDate.ToString());
                    cmd.Parameters.AddWithValue("@param12", "ESS");
                    cmd.Parameters.AddWithValue("@param13", DateTime.Now);
                    cmd.Parameters.AddWithValue("@param14", "Insert");
                    cmd.Parameters.AddWithValue("@param15", 1);

                    _prSqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public async Task WriteAppraisalToHR()
        {

        }

        public async Task WriteTransferToHR(AppliedTransfer transfer)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = _sqlConnection;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = @"INSERT INTO EmpTransfer(Emp_No,OldDeptCode,NewDeptCode, olddivcode, newdivcode, oldseccode, newseccode, MgrStatus, HrStatus) 
                                VALUES(@param1,@param2,@param3, @param4, @param5,@param6,@param7, @param8, @param9)";
                    cmd.Parameters.AddWithValue("@param1", transfer.Emp_No);
                    cmd.Parameters.AddWithValue("@param2", transfer.Employee.Department.DeptCode);
                    cmd.Parameters.AddWithValue("@param3", transfer.Department.DeptCode);
                    cmd.Parameters.AddWithValue("@param4", transfer.Employee.Division.DivisonCode);
                    cmd.Parameters.AddWithValue("@param5", transfer.Division.DivisonCode);
                    cmd.Parameters.AddWithValue("@param6", transfer.Employee.Section.SectionCode);
                    cmd.Parameters.AddWithValue("@param7", transfer.Section.SectionCode);
                    cmd.Parameters.AddWithValue("@param8", 1);
                    cmd.Parameters.AddWithValue("@param9", 0);

                    _sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public async Task WriteAdvanceToHR(PaymentAdvance advance)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    var startDate = new DateTime(advance.TargetDate.Year, advance.TargetDate.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    cmd.Connection = _prSqlConnection;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = @"INSERT INTO PRLNAPRV(emp_no,start_date,end_date,amt_rqst,amt_aprv,no_of_pay,mon_ded,hrstatus,FromDate,ToDate,ValueDate,Insertusername,InsertTransacDate,InsertTransacType, status) 
                                VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14, @param15)";
                    cmd.Parameters.AddWithValue("@param1", advance.Emp_No);
                    cmd.Parameters.AddWithValue("@param2", startDate);
                    cmd.Parameters.AddWithValue("@param3", endDate);
                    cmd.Parameters.AddWithValue("@param4", advance.Amount);
                    cmd.Parameters.AddWithValue("@param5", advance.Amount);
                    cmd.Parameters.AddWithValue("@param6", 1);
                    cmd.Parameters.AddWithValue("@param7", advance.Amount);
                    cmd.Parameters.AddWithValue("@param8", 0);
                    cmd.Parameters.AddWithValue("@param9", startDate.ToString());
                    cmd.Parameters.AddWithValue("@param10", endDate.ToString());
                    cmd.Parameters.AddWithValue("@param11", advance.CreatedDate.ToString());
                    cmd.Parameters.AddWithValue("@param12", "ESS");
                    cmd.Parameters.AddWithValue("@param13", DateTime.Now);
                    cmd.Parameters.AddWithValue("@param14", "Insert");
                    cmd.Parameters.AddWithValue("@param15", 1);

                    _prSqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }
}
