﻿using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IUserRoleService
    {
        Task<BaseResponse> Create(UserRole model);
        Task<BaseResponse> Edit(Guid id, Guid roleId);
        Task<IEnumerable<UserRole>> GetAll();
        Task<BaseResponse> Delete(Guid id);
    }
}
