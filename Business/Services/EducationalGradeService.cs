using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Services
{
    public class EducationalGradeService: IEducationalGradeService
    {
        private readonly ServiceContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;

        public EducationalGradeService(ServiceContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(HRDbConfig.ConnectionStringUrl);
        }

        public async Task<IEnumerable<EducationalGrade>> GetAll(Expression<Func<EducationalGrade, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EducationalGrade>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<IEnumerable<EducationalGrade>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HREducGrade";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HREducGrade> resource = new List<HREducGrade>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HREducGrade requester = new HREducGrade()
                    {
                        Gradecode = reader["Gradecode"].ToString(),
                        Educlevelcode = reader["Educlevelcode"].ToString(),
                        descc = reader["descc"].ToString(),
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }

            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<EducationalGrade>().GetFirstOrDefaultAsync(predicate: x => x.Code.ToLower() == item.Gradecode.ToLower());

                    if (check == null)
                    {
                        var eduGrade = new EducationalGrade() { Code = item.Gradecode, EducationalLevelCode = item.Educlevelcode, Description = item.descc, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };

                        _unitOfWork.GetRepository<EducationalGrade>().Insert(eduGrade);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }
    }
}
