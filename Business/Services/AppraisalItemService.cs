using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class AppraisalItemService: IAppraisalItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppraisalItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(List<AppraisalItem> model)
        {
            foreach(var item in model)
            {
                _unitOfWork.GetRepository<AppraisalItem>().Insert(model);
            }
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
        }

        public async Task<IEnumerable<AppraisalItem>> GetByEmployee(Guid employeeId)
        {
            var data = await _unitOfWork.GetRepository<AppraisalItem>().GetAllAsync(predicate: x => x.EmployeeAppraisal.EmployeeId == employeeId, null, "EmployeeAppraisal.Employee,AppraisalCategory,AppraisalCategoryItem,AppraisalRating");
            return data;
        }

        public async Task<IEnumerable<AppraisalItem>> GetAll()
        {

            var data = await _unitOfWork.GetRepository<AppraisalItem>().GetAllAsync(null, null,"EmployeeAppraisal.Employee,AppraisalCategory,AppraisalCategoryItem,AppraisalRating");
            return data;
        }
    }
}
