using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Services
{
    public class GradeLevelService: IGradeLevelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ServiceContext _dbContext;

        public GradeLevelService(IUnitOfWork unitOfWork, ServiceContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<BaseResponse> Refresh()
        {
            var resource = await _dbContext.HRGrade.ToListAsync();
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<GradeLevel>().GetFirstOrDefaultAsync(predicate: x => x.GradeCode.ToLower() == item.GradeCode.ToLower());

                    if (check == null)
                    {
                        var gradeLevel = new GradeLevel() { GradeCode = item.GradeCode, Descc = item.descc, CreatedDate = DateTime.Now, Id = Guid.NewGuid(), Slot = item.slot };

                        _unitOfWork.GetRepository<GradeLevel>().Insert(gradeLevel);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<GradeLevel>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()), "Employee,Department,Division,Unit,GradeLevel");
            return data;
        }

        public async Task<IEnumerable<GradeLevel>> GetAll(Expression<Func<GradeLevel, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<GradeLevel>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }
    }
}
