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
    public class DepartmentService: IDepartmentService
    {
        private readonly ServiceContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(ServiceContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Department>> GetAll(Expression<Func<Department, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Department>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<BaseResponse> Refresh()
        {
            var resource = await _dbContext.HRDept.ToListAsync();
            if(resource != null)
            {
                foreach(var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<Department>().GetFirstOrDefaultAsync(predicate: x => x.DeptCode.ToLower() == item.DeptCode.ToLower());

                    if (check == null)
                    {
                        var department = new Department() { DeptCode = item.DeptCode, DivisionCode = item.DivisionCode, Descc = item.descc, CompanyCode = item.CompanyCode, CreatedDate = DateTime.Now, Id = Guid.NewGuid(), Slot = item.slot};

                        _unitOfWork.GetRepository<Department>().Insert(department);
                    }
                        
                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }
    }
}
