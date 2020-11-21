using System;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class EmployeeApprovalConfigService: IEmployeeApprovalConfigService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeApprovalConfigService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(EmployeeApprovalConfig model)
        {
            var check = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalLevel == model.ApprovalLevel && x.EmployeeId == model.EmployeeId);
            if(check == null)
            {
                _unitOfWork.GetRepository<EmployeeApprovalConfig>().Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.RecordExist };
        }

        public async Task<BaseResponse> Edit(Guid id, Guid processorIId)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.ProcessorIId = processorIId;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<EmployeeApprovalConfig>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<EmployeeApprovalConfig> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
