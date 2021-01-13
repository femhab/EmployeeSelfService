using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Services
{
    public class DepartmentService: IDepartmentService
    {
        private readonly ServiceContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public DepartmentService(ServiceContext dbContext,IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
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

        public async Task<IEnumerable<Department>> GetByExitApproval()
        {
            var data = await GetAll(x => x.CanClearEmployeeOnExit, "Employee,Employee.Department");
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

        public async Task<BaseResponse> ChangeClearanceRole(Guid departmentId, bool canClear)
        {
            var department = await _unitOfWork.GetRepository<Department>().GetFirstOrDefaultAsync(predicate: x => x.Id == departmentId);
            department.CanClearEmployeeOnExit = canClear;

            _unitOfWork.GetRepository<Department>().Update(department);
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse() { Status = true, Message = ResponseMessage.OperationSuccessful };
        }

        public async Task<BaseResponse> AssignHOD(Guid departmentId, Guid hodId)
        {
            var department = await _unitOfWork.GetRepository<Department>().GetFirstOrDefaultAsync(predicate: x => x.Id == departmentId);
            if(department != null)
            {
                department.HOD = hodId;
                _unitOfWork.GetRepository<Department>().Update(department);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }
    }
}
