using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Business.Interfaces;
using Data.Entities;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class EmployeeApprovalConfigService : IEmployeeApprovalConfigService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeApprovalConfigService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(EmployeeApprovalConfig model)
        {
            if (model != null)
            {
                _unitOfWork.GetRepository<EmployeeApprovalConfig>().Insert(model);
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

        public async Task<BaseResponse> Edit(EmployeeApprovalConfig model)
        {
            var check = await GetById(model.Id);

            if (check != null)
            {
                _unitOfWork.GetRepository<EmployeeApprovalConfig>().Update(model);
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

        public async Task<BaseResponse> Reset()
        {

        }

        public async Task<EmployeeApprovalConfig> GetById(Guid id)
        {
            var data = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            return data;
        }

    }
}
