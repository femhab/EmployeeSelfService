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
    public class RoleService: IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(Role model)
        {
            if (model != null)
            {
                _unitOfWork.GetRepository<Role>().Insert(model);
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

        public async Task<BaseResponse> Edit(Role model)
        {
            var check = await GetById(model.Id);

            if (check != null)
            {
                _unitOfWork.GetRepository<Role>().Update(model);
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

        public async Task<Role> GetById(Guid id)
        {
            var data = await _unitOfWork.GetRepository<Role>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            return data;
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            var data = await _unitOfWork.GetRepository<IEnumerable<Role>>().FindAsync();
            return data;
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var check = await GetById(id);

            if (check != null)
            {
                _unitOfWork.GetRepository<Role>().Delete(check);
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