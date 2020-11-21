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
    public class EmployeeNOKDetailService: IEmployeeNOKDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApprovalBoardService _approvalBoardService;

        public EmployeeNOKDetailService(IUnitOfWork unitOfWork, IApprovalBoardService approvalBoardService)
        {
            _unitOfWork = unitOfWork;
            _approvalBoardService = approvalBoardService;
        }

        public async Task<BaseResponse> Create(EmployeeNOKDetail model)
        {
            var check = await GetAll(x => x.EmployeeId == model.EmployeeId );
            if (check.Count() < 3)
            {
                _unitOfWork.GetRepository<EmployeeNOKDetail>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                //submit for approval
                var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("nextofkin"));
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
            return new BaseResponse() { Status = false, Message = ResponseMessage.MaximumReached };
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var model = await GetById(id);
            if (model != null)
            {
                _unitOfWork.GetRepository<EmployeeNOKDetail>().Delete(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Edit(Guid id, string firstName, string lastName, string email, string phoneNumber, DateTime? dob, string address, Guid relationshipId)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.FirstName = firstName;
                model.LastName = lastName;
                model.Email = email;
                model.PhoneNumber = phoneNumber;
                model.DOB = dob;
                model.Address = address;
                model.RelationshipId = relationshipId;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<EmployeeNOKDetail>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<EmployeeNOKDetail>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<EmployeeNOKDetail> GetByEmployee(Guid employeeId)
        {
            var data = await _unitOfWork.GetRepository<EmployeeNOKDetail>().GetFirstOrDefaultAsync(predicate: x => x.EmployeeId == employeeId);
            return data;
        }

        public async Task<IEnumerable<EmployeeNOKDetail>> GetAll(Expression<Func<EmployeeNOKDetail, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EmployeeNOKDetail>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<EmployeeNOKDetail> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<EmployeeNOKDetail>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
