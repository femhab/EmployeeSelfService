using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class EmployeeFamilyDependentService: IEmployeeFamilyDependentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApprovalBoardService _approvalBoardService;

        public EmployeeFamilyDependentService(IUnitOfWork unitOfWork, IApprovalBoardService approvalBoardService)
        {
            _unitOfWork = unitOfWork;
            _approvalBoardService = approvalBoardService;
        }

        public async Task<BaseResponse> Create(EmployeeFamilyDependent model)
        {
            var check = await GetAll(x => x.EmployeeId == model.EmployeeId);
            if (check.Count() < 3)
            {
                _unitOfWork.GetRepository<EmployeeFamilyDependent>().Insert(model);
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
                _unitOfWork.GetRepository<EmployeeFamilyDependent>().Delete(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Edit(Guid id, string firstName, string lastName, string phoneNumber, DateTime? dob, string address, Guid relationshipId)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.FirstName = firstName;
                model.LastName = lastName;
                model.PhoneNumber = phoneNumber;
                model.DOB = dob;
                model.Address = address;
                model.RelationshipId = relationshipId;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<EmployeeFamilyDependent>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<EmployeeFamilyDependent>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<IEnumerable<EmployeeFamilyDependent>> GetByEmployee(Guid employeeId)
        {
            var data = await GetAll(x => x.EmployeeId == employeeId, "Relationship");
            return data;
        }

        public async Task<IEnumerable<EmployeeFamilyDependent>> GetAll(Expression<Func<EmployeeFamilyDependent, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EmployeeFamilyDependent>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<EmployeeFamilyDependent> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<EmployeeFamilyDependent>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
