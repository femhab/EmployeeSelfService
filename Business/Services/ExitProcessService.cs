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
    public class ExitProcessService: IExitProcessService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExitProcessService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(ExitProcess model)
        {
            if (model != null)
            {
                _unitOfWork.GetRepository<ExitProcess>().Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Update(Guid exitProcessId, ExitProcessStatus status)
        {
            var check = await _unitOfWork.GetRepository<ExitProcess>().GetFirstOrDefaultAsync(predicate: x => x.Id == exitProcessId);
            if(check != null)
            {
                if(check.Status != status)
                {
                    check.Status = status;
                    check.UpdatedDate = DateTime.Now;
                    _unitOfWork.GetRepository<ExitProcess>().Update(check);
                    await _unitOfWork.SaveChangesAsync();
                    return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
                }
                return new BaseResponse() { Status = false, Message = ResponseMessage.AlreadyApproved };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.NoRecordExist };
        }

        public async Task<IEnumerable<ExitProcess>> GetUnapprovedApplication()
        {
            var data = await GetAll(x => x.Status == ExitProcessStatus.Pending, "Employee,Employee.Department");
            return data;
        }

        public async Task<IEnumerable<ExitProcess>> GetAll(Expression<Func<ExitProcess, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<ExitProcess>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<ExitProcess> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<ExitProcess>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
