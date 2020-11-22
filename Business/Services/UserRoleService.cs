using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class UserRoleService: IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(UserRole model)
        {
            if (model != null)
            {
                _unitOfWork.GetRepository<UserRole>().Insert(model);
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

        public async Task<BaseResponse> Edit(UserRole model)
        {
            var check = await GetById(model.Id);

            if (check != null)
            {
                _unitOfWork.GetRepository<UserRole>().Update(model);
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

        public async Task<UserRole> GetById(Guid id)
        {
            var data = await _unitOfWork.GetRepository<UserRole>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            return data;
        }

        public async Task<IEnumerable<UserRole>> GetAll()
        {
            var data = await _unitOfWork.GetRepository<IEnumerable<UserRole>>().FindAsync();
            return data;
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var check = await GetById(id);

            if (check != null)
            {
                _unitOfWork.GetRepository<UserRole>().Delete(check);
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