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
    public class AppraisalRatingService: IAppraisalRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public AppraisalRatingService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
        }

        public async Task<IEnumerable<AppraisalRating>> GetAll(Expression<Func<AppraisalRating, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<AppraisalRating>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Weight));
            return model;
        }

        public async Task<IEnumerable<AppraisalRating>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from AppraisalRating";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<AppraisalRatings> resource = new List<AppraisalRatings>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    AppraisalRatings requester = new AppraisalRatings()
                    {
                        CoreValues = reader["CoreValues"].ToString(),
                        ConceptDefinition = reader["ConceptDefinition"].ToString(),
                        Weight = reader.GetInt32(reader.GetOrdinal("Weight"))
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<AppraisalRating>().GetFirstOrDefaultAsync(predicate: x => x.AppraisalRatingCode.ToLower() == item.CoreValues.ToLower());

                    if (check == null)
                    {
                        var appraisal = new AppraisalRating() { Description = item.ConceptDefinition, AppraisalRatingCode = item.CoreValues, Weight = item.Weight, CreatedDate = DateTime.Now, Id = Guid.NewGuid()};
                        _unitOfWork.GetRepository<AppraisalRating>().Insert(appraisal);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }
    }
}
