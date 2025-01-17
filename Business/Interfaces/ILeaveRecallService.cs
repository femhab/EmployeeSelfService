﻿using Data.Entities;
using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface ILeaveRecallService
    {
        Task<BaseResponse> Create(LeaveRecall model);
        Task<LeaveRecall> GetById(Guid id);
    }
}
