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

        public LeaveService(IUnitOfWork unitOfWork, IApprovalBoardService approvalBoardService)
        {
            _unitOfWork = unitOfWork;
            _approvalBoardService = approvalBoardService;
        }

        public async Task<BaseResponse> Create(Leave model)
        {
            //check if there is an existing leave
            var check = await _unitOfWork.GetRepository<Leave>().GetFirstOrDefaultAsync(predicate: x => x.CreatedDate.Year == DateTime.Now.Year && x.LeaveStatus != LeaveStatus.UnApproved);
            if(check == null)
            {
                _unitOfWork.GetRepository<Leave>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                //submit for approval
                var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("leave"));
                var approvalProcessor = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalLevel == Level.HR);

                await _approvalBoardService.Create(new ApprovalBoard()
                {
                    EmployeeId = model.EmployeeId,
                    ApprovalLevel = Level.HR,
                    Emp_No = model.Emp_No,
                    ApprovalWorkItemId = approvalWorkItem.Id,
                    ApprovalProcessorId = approvalProcessor.Id,
                    ServiceId = model.Id,
                    Status = ApprovalStatus.Pending,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    CreatedBy = model.Emp_No
                });

                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.LeaveExist };
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
                if(model.ActualEndDate > dateTo)
                {
                    model.DateFrom = startDate;
                    model.DateTo = dateTo;
                    model.UpdatedDate = DateTime.Now;

                    _unitOfWork.GetRepository<Leave>().Update(model);
                    await _unitOfWork.SaveChangesAsync();
                    return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful };
                }
                return new BaseResponse() { Status = false, Message = ResponseMessage.MaximumLeaveReached };
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
            var data = await GetAll(x => x.EmployeeId == employeeId);
            return data;
        }

        public async Task<IEnumerable<Leave>> GetEmployeeOnLeave()
        {
            var data = await GetAll(x => x.CreatedDate.Year == DateTime.Now.Year && x.LeaveStatus == LeaveStatus.OnLeave);
            return data;
        }

        public async Task<IEnumerable<Leave>> GetAll(Expression<Func<Leave, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Leave>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), "Employee");
            return model;
        }

        public async Task<Leave> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<Leave>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
