using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;


namespace Business.Services
{
    public class EducationalLevelService: IEducationalLevelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ServiceContext _dbContext;

        public EducationalLevelService(IUnitOfWork unitOfWork, ServiceContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<EducationalLevel>> GetAll(Expression<Func<EducationalLevel, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EducationalLevel>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<IEnumerable<EducationalLevel>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<BaseResponse> Refresh()
        {
            var resource = await _dbContext.HREducLevel.ToListAsync();
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<EducationalLevel>().GetFirstOrDefaultAsync(predicate: x => x.Code.ToLower() == item.EducLevelCode.ToLower());

                    if (check == null)
                    {
                        var eduLevel = new EducationalLevel() { Code = item.EducLevelCode, Description = item.descc, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };

                        _unitOfWork.GetRepository<EducationalLevel>().Insert(eduLevel);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }
    }
}
