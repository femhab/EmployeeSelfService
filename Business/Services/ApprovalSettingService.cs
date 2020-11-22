using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class ApprovalSettingService: IApprovalSettingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApprovalSettingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(ApprovalSetting model)
        {
            if (model != null)
            {
                _unitOfWork.GetRepository<ApprovalSetting>().Insert(model);
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

        public async Task<BaseResponse> Edit(ApprovalSetting model)
        {
            var check = await GetById(model.Id);

            if (check != null)
            {
                _unitOfWork.GetRepository<ApprovalSetting>().Update(model);
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


        public async Task<ApprovalSetting> GetById(Guid id)
        {
            var data = await _unitOfWork.GetRepository<ApprovalSetting>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            return data;
        }

        public async Task<IEnumerable<ApprovalSetting>> GetAll()
        {
            var data = await _unitOfWork.GetRepository<IEnumerable<ApprovalSetting>>().FindAsync();
            return data;
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var check = await GetById(id);

            if (check != null)
            {
                _unitOfWork.GetRepository<ApprovalSetting>().Delete(check);
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
