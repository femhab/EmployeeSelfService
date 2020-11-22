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
    public class EmployeeEducationalDetailService : IEmployeeEducationalDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeEducationalDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(EmployeeEducationDetail model)
        {
            if (model != null)
            {
                _unitOfWork.GetRepository<EmployeeEducationDetail>().Insert(model);
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

        public async Task<BaseResponse> Edit(EmployeeEducationDetail model)
        {
            var check = await GetById(model.Id);

            if (check != null)
            {
                _unitOfWork.GetRepository<EmployeeEducationDetail>().Update(model);
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

        public async Task<IEnumerable<EmployeeEducationDetail>> GetAll()
        {
            var data = await _unitOfWork.GetRepository<IEnumerable<EmployeeEducationDetail>>().FindAsync();
            return data;
        }

        public async Task<EmployeeEducationDetail> GetByEmployee(Guid employeeId)
        {
            var data = await _unitOfWork.GetRepository<Employee>().GetAllAsync(predicate: x => x.EmployeeId == employeelId);
            return  data;
        }
    }
}
