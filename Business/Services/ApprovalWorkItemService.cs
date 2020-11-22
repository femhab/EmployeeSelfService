using System;
using System.Collections.Generic;
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
            if (model != null)
            {
                _unitOfWork.GetRepository<ApprovalWorkItem>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                BaseResponse response = new BaseResponse
                {
                    Status = true,
                    Message = "Created Successfully"
                };
                return response;
            }

            BaseResponse failresponse = new BaseResponse
            {
                Status = false,
                Message = "Not Created Successfully"
            };
            return failresponse;
        }

        public async Task<BaseResponse> Edit(ApprovalWorkItem model)
        {
            var check = await GetById(model.Id);

            if (check != null)
            {
                _unitOfWork.GetRepository<ApprovalWorkItem>().Update(model);
                await _unitOfWork.SaveChangesAsync();

                BaseResponse response = new BaseResponse
                {
                    Status = true,
                    Message = "Edited Successfully"
                };
                return response;
            }

            BaseResponse failresponse = new BaseResponse
            {
                Status = false,
                Message = "Not Edited Successfully"
            };
            return failresponse;
        }

        public async Task<ApprovalWorkItem> GetById(Guid id)
        {
            var data = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            return data;
        }

        public async Task<IEnumerable<ApprovalWorkItem>> GetAll()
        {
            var data = await _unitOfWork.GetRepository<IEnumerable<ApprovalWorkItem>>().FindAsync();
            return data;
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var check = await GetById(id);

            if (check != null)
            {
                _unitOfWork.GetRepository<ApprovalWorkItem>().Delete(check);
                await _unitOfWork.SaveChangesAsync();


                BaseResponse response = new BaseResponse
                {
                    Status = true,
                    Message = "Deleted Successfully"
                };
                return response;
            }

            BaseResponse failresponse = new BaseResponse
            {
                Status = false,
                Message = "Not Deleted Successfully"
            };
            return failresponse;
        }
    }
}
