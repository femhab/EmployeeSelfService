using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class RoleService: IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(HRDbConfig.ConnectionStringUrl);
        }

        public async Task<BaseResponse> Create(Role model)
        {
            _unitOfWork.GetRepository<Role>().Insert(model);
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var model = await GetById(id);
            if (model != null)
            {
                _unitOfWork.GetRepository<Role>().Delete(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Edit(Guid id, string description)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.Description = description;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<Role>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HRRoles";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRRoles> roleList = new List<HRRoles>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRRoles requester = new HRRoles()
                    {
                        RoleID = reader.GetInt32(reader.GetOrdinal("RoleID")),
                        Description = reader["Description"].ToString(),
                    };
                    roleList.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (roleList != null)
            {
                foreach (var item in roleList)
                {
                    var check = await _unitOfWork.GetRepository<Role>().GetFirstOrDefaultAsync(predicate: x => x.RoleId == item.RoleID);

                    if (check == null)
                    {
                        var role = new Role() { Description = item.Description, RoleId = item.RoleID, CreatedDate = DateTime.Now, Id = Guid.NewGuid()};

                        _unitOfWork.GetRepository<Role>().Insert(role);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<IEnumerable<Role>> GetAll(Expression<Func<Role, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Role>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<Role> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<Role>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
