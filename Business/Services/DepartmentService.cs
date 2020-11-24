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
    public class DepartmentService: IDepartmentService
    {
        private readonly ServiceContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;

        public DepartmentService(ServiceContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(HRDbConfig.ConnectionStringUrl);
        }

        public async Task<IEnumerable<Department>> GetAll(Expression<Func<Department, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Department>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HRDept";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRDept> resource = new List<HRDept>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRDept requester = new HRDept()
                    {
                        DeptCode = reader["DeptCode"].ToString(),
                        CompanyCode = reader["CompanyCode"].ToString(),
                        DivisionCode = reader["DivisionCode"].ToString(),
                        descc = reader["descc"].ToString(),
                        slot = reader.GetInt32(reader.GetOrdinal("slot"))
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
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
