using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class EmployeeEducationalDetailService : IEmployeeEducationalDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeEducationalDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async  Task<BaseResponse> Create(EmployeeEducationDetail model)
        {
            var check = await GetAll(x => x.EmployeeId == model.EmployeeId && x.EducationalQualificationId == model.EducationalQualificationId);
            if (check.Count() < 3)
            {
                _unitOfWork.GetRepository<EmployeeEducationDetail>().Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.MaximumReached };
        }

        public async Task<BaseResponse> Edit(Guid id, Guid levelId, Guid qualificationId, Guid gradeId)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.EducationalLevelId = levelId;
                model.EducationalQualificationId = qualificationId;
                model.EducationalGradeId = gradeId;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<EmployeeEducationDetail>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<EmployeeEducationDetail>> GetByEmployee(Guid employeeId)
        {
            var data = await GetAll(x => x.EmployeeId != employeeId, "Employee,EducationalLevel,EducationalQualification,EducationalGrade");
            return data;
        }

        public async Task<IEnumerable<EmployeeEducationDetail>> GetAll(Expression<Func<EmployeeEducationDetail, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EmployeeEducationDetail>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<EmployeeEducationDetail> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<EmployeeEducationDetail>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
