using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class ApprovalWorkItemService: IApprovalWorkItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApprovalWorkItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(ApprovalWorkItem model)
        {
            _unitOfWork.GetRepository<ApprovalWorkItem>().Insert(model);
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var model = await GetById(id);
            if (model != null)
            {
                _unitOfWork.GetRepository<Department>().Delete(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Edit(Guid id, string name, string description)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.Name = name;
                model.Description = description;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<ApprovalWorkItem>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<ApprovalWorkItem>> GetAll(Expression<Func<ApprovalWorkItem, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<ApprovalWorkItem> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
