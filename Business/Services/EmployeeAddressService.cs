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
    public class EmployeeAddressService : IEmployeeAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeAddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(EmployeeAddress model)
        {
            if (model != null)
            {
                _unitOfWork.GetRepository<EmployeeAddress>().Insert(model);
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

        public async Task<BaseResponse> Edit(EmployeeAddress model)
        {
            var check = await GetById(model.Id);

            if (check != null)
            {
                _unitOfWork.GetRepository<EmployeeAddress>().Update(model);
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

        public async Task<EmployeeAddress> GetById(Guid id)
        {
            var data = await _unitOfWork.GetRepository<EmployeeAddress>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            return data;
        }

        public async Task<IEnumerable<EmployeeAddress>> GetAll()
        {
            var data = await _unitOfWork.GetRepository<IEnumerable<EmployeeAddress>>().FindAsync();
            return data;
        }

        public async Task<IEnumerable<EmployeeAddress>> GetByDepartment(Guid departmentId)
        {
            var data = await _unitOfWork.GetRepository<Employee>().GetAllAsync(predicate: x => x.EmployeeId == employeelId);
            return data;
        }

        public async Task<EmployeeAddress> GetByEmployee(Guid employeeId)
        {
            var data = await _unitOfWork.GetRepository<Employee>().GetAllAsync(predicate: x => x.EmployeeId == employeelId);
            return data;
        }
    }
}