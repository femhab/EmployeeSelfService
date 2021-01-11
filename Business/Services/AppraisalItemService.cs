using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ViewModel.Model;
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

        public async Task<BaseResponse> Update(List<AppraisalItemUpdateModel> model)
        {
            try
            {
                var updateList = new List<AppraisalItem>();
                foreach (var item in model)
                {
                    var check = await _unitOfWork.GetRepository<AppraisalItem>().GetFirstOrDefaultAsync(predicate: x => x.EmployeeAppraisalId == item.EmployeeAppraisalId && x.AppraisalCategoryItemId == item.CategoryItemId);
                    if (check != null)
                    {
                        check.AppraisalRatingId = item.RatingId;
                        check.UpdatedDate = DateTime.Now;
                        updateList.Add(check);
                    }
                }
                _unitOfWork.GetRepository<AppraisalItem>().Update(updateList);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
        }

        public async Task<IEnumerable<AppraisalItem>> GetByEmployee(Guid employeeId)
        {
            var data = await _unitOfWork.GetRepository<AppraisalItem>().GetAllAsync(predicate: x => x.EmployeeAppraisal.EmployeeId == employeeId, null, "EmployeeAppraisal.Employee,AppraisalCategory,AppraisalCategoryItem,AppraisalRating");
            return data;
        }

        public async Task<IEnumerable<AppraisalItem>> GetByAppraisal(Guid appraisalId)
        {
            var data = await _unitOfWork.GetRepository<AppraisalItem>().GetAllAsync(predicate: x => x.EmployeeAppraisalId == appraisalId, null, "EmployeeAppraisal.Employee,AppraisalCategory,AppraisalCategoryItem,AppraisalRating");
            return data;
        }

        public async Task<IEnumerable<AppraisalItem>> GetAll()
        {

            var data = await _unitOfWork.GetRepository<AppraisalItem>().GetAllAsync(null, null,"EmployeeAppraisal.Employee,AppraisalCategory,AppraisalCategoryItem,AppraisalRating");
            return data;
        }
    }
}
