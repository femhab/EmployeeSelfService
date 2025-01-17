﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class EmployeeAddressService: IEmployeeAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApprovalBoardService _approvalBoardService;

        public EmployeeAddressService(IUnitOfWork unitOfWork, IApprovalBoardService approvalBoardService)
        {
            _unitOfWork = unitOfWork;
            _approvalBoardService = approvalBoardService;
        }

        public async Task<BaseResponse> Create(EmployeeAddress model)
        {
            var check = await _unitOfWork.GetRepository<EmployeeAddress>().GetFirstOrDefaultAsync(predicate: x => x.EmployeeId == model.EmployeeId);
            if(check == null)
            {
                _unitOfWork.GetRepository<EmployeeAddress>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                //submit for approval
                var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("address"));
                var approvalProcessor = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalLevel == Level.HR);

                await _approvalBoardService.Create(new ApprovalBoard()
                {
                    EmployeeId = model.EmployeeId,
                    ApprovalLevel = Level.HR,
                    Emp_No = model.Emp_No,
                    ApprovalWorkItemId = approvalWorkItem.Id,
                    ApprovalProcessorId = approvalProcessor.Id,
                    ServiceId = model.Id,
                    Status = ApprovalStatus.Pending,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    CreatedBy = model.Emp_No
                });

                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.RecordExist };
        }

        public async Task<BaseResponse> Edit(Guid id, string streetAddress, string state, string city, string country, string stateOfOrigin, string lGOfOrigin)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.StreetAddress = streetAddress;
                model.State = state;
                model.City = city;
                model.Country = country;
                model.StateOfOrigin = stateOfOrigin;
                model.LGOfOrigin = lGOfOrigin;
                model.Status = ApprovalStatus.Pending;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<EmployeeAddress>().Update(model);
                await _unitOfWork.SaveChangesAsync();

                //submit for approval
                var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("address"));
                var approvalProcessor = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalLevel == Level.HR);

                await _approvalBoardService.Create(new ApprovalBoard()
                {
                    EmployeeId = model.EmployeeId,
                    ApprovalLevel = Level.HR,
                    Emp_No = model.Emp_No,
                    ApprovalWorkItemId = approvalWorkItem.Id,
                    ApprovalProcessorId = approvalProcessor.Id,
                    ServiceId = model.Id,
                    Status = ApprovalStatus.Pending,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    CreatedBy = model.Emp_No
                });

                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<EmployeeAddress>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<IEnumerable<EmployeeAddress>> GetAll(Expression<Func<EmployeeAddress, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EmployeeAddress>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<IEnumerable<EmployeeAddress>> GetByDepartment(Guid departmentId)
        {
            var data = await GetAll(x => x.Emp_No != null, "Employee");
            var response = data.Where(x => x.Employee.DepartmentId == departmentId);

            return response;
        }

        public async Task<EmployeeAddress> GetByEmployee(Guid employeeId)
        {
            var data = await _unitOfWork.GetRepository<EmployeeAddress>().GetFirstOrDefaultAsync(predicate: x => x.EmployeeId == employeeId);
            return data;
        }

        public async Task<EmployeeAddress> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<EmployeeAddress>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
