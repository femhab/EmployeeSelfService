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
    public class LoanTypeService: ILoanTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoanTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(LoanType model)
        {
            var check = await _unitOfWork.GetRepository<LoanType>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower() == model.Name.ToLower());
            if (check == null)
            {
                _unitOfWork.GetRepository<LoanType>().Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.RecordExist };
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var model = await GetById(id);
            if (model != null)
            {
                _unitOfWork.GetRepository<LoanType>().Delete(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<LoanType>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<BaseResponse> Edit(Guid id, string name)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.Name = name;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<LoanType>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<LoanType>> GetAll(Expression<Func<LoanType, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<LoanType>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<LoanType> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<LoanType>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
