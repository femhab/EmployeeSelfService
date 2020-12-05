using System;
using System.Collections.Generic;
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

        public async Task<BaseResponse> CreateUpdate(List<EmployeeApprovalConfig> model)
        {
            if(model != null)
            {
                foreach (var item in model)
                {
                    var check = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalLevel == item.ApprovalLevel && x.EmployeeId == item.EmployeeId);
                    if (check == null)
                    {
                        _unitOfWork.GetRepository<EmployeeApprovalConfig>().Insert(model);
                    }
                    else
                    {
                        check.ProcessorIId = item.ProcessorIId;
                        item.UpdatedDate = DateTime.Now;

                        _unitOfWork.GetRepository<EmployeeApprovalConfig>().Update(check);
                    }
                    await _unitOfWork.SaveChangesAsync();
                    return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
                }
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<EmployeeApprovalConfig> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }

        public async Task<BaseResponse> SetApprovalCount(EmployeeApprovalCount model)
        {
            var check = await _unitOfWork.GetRepository<EmployeeApprovalCount>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalWorkItemId == model.ApprovalWorkItemId && x.EmployeeId == model.EmployeeId);
            if (check == null)
            {
                _unitOfWork.GetRepository<EmployeeApprovalCount>().Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            else
            {
                check.MaximumCount = model.MaximumCount;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<EmployeeApprovalCount>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
            }
        }

        public async Task<int> GetApprovalCount(Guid employeeId, Guid approvalWorkItemId)
        {
            var model = await _unitOfWork.GetRepository<EmployeeApprovalCount>().GetFirstOrDefaultAsync(predicate: c => c.EmployeeId == employeeId && c.ApprovalWorkItemId == approvalWorkItemId);
            return model.MaximumCount;
        }
    }
}
