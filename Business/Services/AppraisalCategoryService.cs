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
    public class AppraisalCategoryService: IAppraisalCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public AppraisalCategoryService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
        }

        public async Task<IEnumerable<AppraisalCategory>> GetAll(Expression<Func<AppraisalCategory, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<AppraisalCategory>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.OrderNo));
            return model;
        }

        public async Task<IEnumerable<AppraisalCategory>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from AppraisalCategory";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<AppraisalCategorries> resource = new List<AppraisalCategorries>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    AppraisalCategorries requester = new AppraisalCategorries()
                    {
                        AppraisalCategory = reader["AppraisalCategory"].ToString(),
                        AppraisalCategoryCode = reader["AppraisalCategoryCode"].ToString(),
                        OrderNo = reader["OrderNo"].ToString()
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<AppraisalCategory>().GetFirstOrDefaultAsync(predicate: x => x.AppraisalCategoryCode.ToLower() == item.AppraisalCategoryCode.ToLower());

                    if (check == null)
                    {
                        var appraisalCategory = new AppraisalCategory() { AppraisalCategoryCode = item.AppraisalCategoryCode, Description = item.AppraisalCategory, OrderNo = int.Parse(item.OrderNo), CreatedDate = DateTime.Now, Id = Guid.NewGuid() };

                        _unitOfWork.GetRepository<AppraisalCategory>().Insert(appraisalCategory);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }
    }
}
