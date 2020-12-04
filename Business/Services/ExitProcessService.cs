using System;
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
            if (model == null)
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
    }
}
