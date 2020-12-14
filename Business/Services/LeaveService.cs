using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class LeaveService: ILeaveService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IApprovalBoardActiveLevelService _approvalBoardActiveLevelService;
        private readonly IEmployeeService _employeeService;
        private readonly ILeaveTypeService _leaveTypeService;

        public LeaveService(IUnitOfWork unitOfWork, IApprovalBoardService approvalBoardService, IEmployeeService employeeService, ILeaveTypeService leaveTypeService, IEmployeeApprovalConfigService employeeApprovalConfigService, IApprovalBoardActiveLevelService approvalBoardActiveLevelService)
        {
            _unitOfWork = unitOfWork;
            _approvalBoardService = approvalBoardService;
            _employeeService = employeeService;
            _leaveTypeService = leaveTypeService;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _approvalBoardActiveLevelService = approvalBoardActiveLevelService;
        }

        public async Task<BaseResponse> Create(Leave model)
        {
            //check if there is an existing leave
            var check = await _unitOfWork.GetRepository<Leave>().GetFirstOrDefaultAsync(predicate: x => x.CreatedDate.Year == DateTime.Now.Year && x.LeaveStatus != LeaveStatus.Rejected);
            if(check == null)
            {
                _unitOfWork.GetRepository<Leave>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                //submit for approval
                var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("leave"));
                var approvalProcessor = await _employeeApprovalConfigService.GetBy(x => x.EmployeeId == model.EmployeeId && x.ApprovalLevel == Level.FirstLevel && x.ApprovalWorkItemId == approvalWorkItem.Id);
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

                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.LeaveExist };
        }

        public async Task<LeaveResponseModel> CheckEligibility(Guid employeeId)
        {
            var employee = await _employeeService.GetById(employeeId);
            bool IsFiveYearsAnn = false;
            bool IsTenYearsAnn = false;
            if (employee != null && employee.EmploymentDate != null)
            {
                IsFiveYearsAnn = (employee.EmploymentDate.Value.AddYears(5).Year == DateTime.Now.Year) ? true : false;
                IsTenYearsAnn = (employee.EmploymentDate.Value.AddYears(10).Year == DateTime.Now.Year) ? true : false;
            }
            var employeeLeaveAudit = await GetAllEmployeeLeave(x => x.EmployeeId == employeeId);
            List<LeaveTypeAudit> eligibleDays = new List<LeaveTypeAudit>();
            foreach(var item in employeeLeaveAudit)
            {
                var leaveAudit = new LeaveTypeAudit() { LeaveType = item.LeaveTypeId, NoDaysRemaining = item.NoOfEligibleDays + item.AnnivessaryLeaveBonus - item.NoOfDaysUsed };
                eligibleDays.Add(leaveAudit);
            }
            return new LeaveResponseModel() { Status = true, Message = ResponseMessage.OperationSuccessful, IsFiveYearsAnniversary = IsFiveYearsAnn, IsTenYearsAnniversary = IsTenYearsAnn, LeaveTypeAudit = eligibleDays };
        }

        //To be run on employee creation
        public async Task EmployeeLeavePreset(Guid employeeId, string empNo, Guid gradeLevelId)
        {
            var leaveTypeResponse = await _leaveTypeService.GetAll();
            var leaveTypes = leaveTypeResponse.Where(x => x.GradeLevelId == gradeLevelId);

            foreach(var item in leaveTypes)
            {
                var employeeLeave = new EmployeeLeave()
                {
                    EmployeeId = employeeId,
                    Emp_No = empNo,
                    LeaveTypeId = item.Id,
                    NoOfEligibleDays = item.AvailableDays,
                    NoOfDaysUsed = 0,
                    AnnivessaryLeaveBonus = 0
                };
                _unitOfWork.GetRepository<EmployeeLeave>().Insert(employeeLeave);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        //To be run every Jan 1st
        public async Task ResetEmployeeLeave()
        {
            var employeeLeave = await GetAllEmployeeLeave(x => x.Id != null);
            var leaveTypeResponse = await _leaveTypeService.GetAll();

            foreach (var item in employeeLeave)
            {
                var employee = await _employeeService.GetById(item.EmployeeId);
                var IsFiveYearsAnn = (employee.EmploymentDate.Value.AddYears(5).Year == DateTime.Now.Year) ? true : false;
                var IsTenYearsAnn = (employee.EmploymentDate.Value.AddYears(10).Year == DateTime.Now.Year) ? true : false;

                var leaveTypes = leaveTypeResponse.Where(x => x.GradeLevelId == employee.GradeLevelId);
                foreach (var data in leaveTypes)
                {

                    item.NoOfEligibleDays = data.AvailableDays;
                    item.NoOfDaysUsed = 0;
                    if (IsFiveYearsAnn)
                        item.AnnivessaryLeaveBonus = 3; //to be put on appsettings
                    if (IsTenYearsAnn)
                        item.AnnivessaryLeaveBonus = 5; //to be put on appsettings
                    if (!IsFiveYearsAnn && !IsTenYearsAnn)
                        item.AnnivessaryLeaveBonus = 0;

                    _unitOfWork.GetRepository<EmployeeLeave>().Update(item);
                }
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var model = await GetById(id);
            if (model != null && model.LeaveStatus == LeaveStatus.Pending)
            {
                _unitOfWork.GetRepository<Leave>().Delete(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.LeaveExecuted };
        }

        public async Task<BaseResponse> Edit(Guid id, DateTime startDate, int noOfDays)
        {
            var model = await GetById(id);
            if (model != null)
            {
                var dateTo = startDate.AddDays(noOfDays);
                if(model.LeaveStatus == LeaveStatus.Pending)
                {
                    model.DateFrom = startDate;
                    model.DateTo = dateTo;
                    model.UpdatedDate = DateTime.Now;

                    _unitOfWork.GetRepository<Leave>().Update(model);
                    await _unitOfWork.SaveChangesAsync();
                    return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful };
                }
                return new BaseResponse() { Status = false, Message = ResponseMessage.LeaveExecuted };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<Leave>> GetApprovedLeave()
        {
            var data = await GetAll(x => x.CreatedDate.Year == DateTime.Now.Year && x.DateTo < DateTime.Now && x.LeaveStatus == LeaveStatus.Approved);
            return data;
        }

        public async Task<IEnumerable<Leave>> GetByDepartment(Guid departmentId)
        {
            var data = await GetAll(x => x.Employee.DepartmentId == departmentId);
            return data;
        }

        public async Task<IEnumerable<Leave>> GetByEmployee(Guid employeeId)
        {
            var data = await GetAll(x => x.EmployeeId == employeeId, "LeaveType");
            return data;
        }

        public async Task<IEnumerable<Leave>> GetEmployeeOnLeave()
        {
            var data = await GetAll(x => x.CreatedDate.Year == DateTime.Now.Year && x.LeaveStatus == LeaveStatus.OnLeave);
            return data;
        }

        public async Task<IEnumerable<Leave>> GetAll(Expression<Func<Leave, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Leave>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<IEnumerable<EmployeeLeave>> GetAllEmployeeLeave(Expression<Func<EmployeeLeave, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EmployeeLeave>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<Leave> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<Leave>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
