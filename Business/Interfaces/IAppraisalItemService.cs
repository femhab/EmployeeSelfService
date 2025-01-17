﻿using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Model;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IAppraisalItemService
    {
        Task<BaseResponse> Create(List<AppraisalItem> model);
        Task<IEnumerable<AppraisalItem>> GetByEmployee(Guid employeeId);
        Task<IEnumerable<AppraisalItem>> GetByAppraisal(Guid appraisalId);
        Task<IEnumerable<AppraisalItem>> GetAll();
        Task<BaseResponse> Update(List<AppraisalItemUpdateModel> model);
    }
}
