﻿using Data.Entities;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IEmployeeApprovalConfigService
    {
        Task<BaseResponse> CreateUpdate(List<EmployeeApprovalConfig> model);
        Task<BaseResponse> SetApprovalCount(EmployeeApprovalCount model);
        Task<int> GetApprovalCount(Guid employeeId, Guid approvalWorkItemId);
        Task<IEnumerable<EmployeeApprovalConfig>> GetByEmployee(Guid employeeId);
        Task<EmployeeApprovalConfig> GetBy(Expression<Func<EmployeeApprovalConfig, bool>> predicate);
        Task<EmployeeApprovalConfig> GetByServiceLevel(Guid employeeId, string approverWorkItem, Level level);
    }
}
