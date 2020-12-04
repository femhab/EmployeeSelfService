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
    public class GradeLevelService: IGradeLevelService
    {
        private readonly ServiceContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;

        public GradeLevelService(ServiceContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(HRDbConfig.ConnectionStringUrl);
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HRGrade";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRGrade> resource = new List<HRGrade>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRGrade requester = new HRGrade()
                    {
                        GradeCode = reader["GradeCode"].ToString(),
                        descc = reader["descc"].ToString(),
                        slot = reader.GetDecimal(reader.GetOrdinal("slot"))
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<GradeLevel>().GetFirstOrDefaultAsync(predicate: x => x.GradeCode.ToLower() == item.GradeCode.ToLower());

                    if (check == null)
                    {
                        var gradeLevel = new GradeLevel() { GradeCode = item.GradeCode, Descc = item.descc, CreatedDate = DateTime.Now, Id = Guid.NewGuid(), Slot = (int)item.slot };

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
