using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Services
{
    public class LeaveTypeService: ILeaveTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ServiceContext _dbContext;

        public LeaveTypeService(IUnitOfWork unitOfWork, ServiceContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<BaseResponse> Edit(Guid id, int availableDays)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.AvailableDays = availableDays;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<LeaveType>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<LeaveType>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<BaseResponse> Refresh()
        {
            var hrData = await _dbContext.hrleavedays.ToListAsync();
            if(hrData != null)
            {
                foreach (var item in hrData)
                {
                    var gradeLevel = await _unitOfWork.GetRepository<GradeLevel>().GetFirstOrDefaultAsync(predicate: x => x.GradeCode.ToLower() == item.gradeCode.ToLower());
                    //for annual
                    var model = new LeaveType() { GradeLevelId = gradeLevel.Id, AvailableDays = item.leaveDays, Class = LeaveClass.Annual, CreatedDate = DateTime.Now };
                    _unitOfWork.GetRepository<LeaveType>().Insert(model);
                    //for others
                    //
                    model.AvailableDays = 0;
                    model.Class = LeaveClass.Casual;
                    _unitOfWork.GetRepository<LeaveType>().Insert(model);
                    //
                    model.AvailableDays = 0;
                    model.Class = LeaveClass.Exam;
                    _unitOfWork.GetRepository<LeaveType>().Insert(model);
                    //
                    model.AvailableDays = 0;
                    model.Class = LeaveClass.Maternity;
                    _unitOfWork.GetRepository<LeaveType>().Insert(model);
                    //
                    model.AvailableDays = 0;
                    model.Class = LeaveClass.Sick;
                    _unitOfWork.GetRepository<LeaveType>().Insert(model);
                    //
                    model.AvailableDays = 0;
                    model.Class = LeaveClass.Study;
                    _unitOfWork.GetRepository<LeaveType>().Insert(model);
                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<LeaveType>> GetAll(Expression<Func<LeaveType, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<LeaveType>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), "Employee");
            return model;
        }

        public async Task<LeaveType> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<LeaveType>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
