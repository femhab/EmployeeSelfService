using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class EducationalQualificationService: IEducationalQualificationService
    {
        private readonly ServiceContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public EducationalQualificationService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
        }

        public async Task<IEnumerable<EducationalQualification>> GetAll(Expression<Func<EducationalQualification, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EducationalQualification>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<IEnumerable<EducationalQualification>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HREducQual";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HREducQual> resource = new List<HREducQual>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HREducQual requester = new HREducQual()
                    {
                        Educlevelcode = reader["Educlevelcode"].ToString(),
                        descc = reader["descc"].ToString(),
                        qualcode = reader["qualcode"].ToString(),
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<EducationalQualification>().GetFirstOrDefaultAsync(predicate: x => x.Code.ToLower() == item.qualcode.ToLower());

                    if (check == null)
                    {
                        var eduQual = new EducationalQualification() { Code = item.qualcode, EducationalLevelCode = item.Educlevelcode, Description = item.descc, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };

                        _unitOfWork.GetRepository<EducationalQualification>().Insert(eduQual);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }
    }
}
