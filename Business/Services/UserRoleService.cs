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
    public class UserRoleService: IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(UserRole model)
        {
            try
            {
                var check = await _unitOfWork.GetRepository<UserRole>().GetFirstOrDefaultAsync(predicate: x => x.EmployeeId == model.EmployeeId && x.RoleId == model.RoleId);
                if (check == null) //
                {
                    _unitOfWork.GetRepository<UserRole>().Insert(model);
                    await _unitOfWork.SaveChangesAsync();
                    return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
                }
                return new BaseResponse() { Status = false, Message = ResponseMessage.RecordExist };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var model = await GetById(id);
            if (model != null)
            {
                _unitOfWork.GetRepository<UserRole>().Delete(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Edit(Guid id, Guid roleId)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.RoleId = roleId;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<UserRole>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<UserRole>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()), "Employee,Role");
            return data;
        }

        public async Task<UserRole> GetByClearanceRole(Guid employeeId)
        {
            var data = (await GetAll(x => x.EmployeeId == employeeId && x.Role.Description.Contains("clearance"), "Employee,Role")).FirstOrDefault();
            return data;
        }

        public async Task<IEnumerable<UserRole>> GetAll(Expression<Func<UserRole, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<UserRole>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<UserRole> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<UserRole>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
