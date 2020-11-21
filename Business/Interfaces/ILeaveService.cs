﻿using Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface ILeaveService
    {
        Task<BaseResponse> Create(Leave model);
        Task<IEnumerable<Leave>> GetByEmployee(Guid employeeId);
        Task<IEnumerable<Leave>> GetByDepartment(Guid departmentId);
        Task<IEnumerable<Leave>> GetEmployeeOnLeave(); //onleave
        Task<IEnumerable<Leave>> GetApprovedLeave(); //approved but not started
        Task<BaseResponse> Edit(Guid id, DateTime startDate, int noOfDays);
        Task<BaseResponse> Delete(Guid id);
    }
}
