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
    public class AppraisalCategoryItemService: IAppraisalCategoryItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public AppraisalCategoryItemService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
        }

        public async Task<IEnumerable<AppraisalCategoryItem>> GetAll(Expression<Func<AppraisalCategoryItem, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<AppraisalCategoryItem>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.AppraisalCategoryCode), include);
            return model;
        }

        public async Task<IEnumerable<AppraisalCategoryItem>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()), "AppraisalCategory");
            return data;
        }

        public async Task<IEnumerable<AppraisalCategoryItem>> GetByStaffType(string staffType, string appraisalCategoryCode)
        {
            var data = await GetAll(x => x.StaffType.ToLower() == staffType && x.AppraisalCategoryCode.ToLower() == appraisalCategoryCode);
            return data;
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from AppraisalCategoryItem";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<AppraisalCategoryItems> resource = new List<AppraisalCategoryItems>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    AppraisalCategoryItems requester = new AppraisalCategoryItems()
                    {
                        Desccription = reader["Desccription"].ToString(),
                        AppraisalCategoryCode = reader["AppraisalCategoryCode"].ToString(),
                        TypeCode = reader["TypeCode"].ToString(),
                        Weight = reader.GetInt32(reader.GetOrdinal("Weight")),
                        AppraisalCategoryItemID = reader.GetInt32(reader.GetOrdinal("AppraisalCategoryItemID"))
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<AppraisalCategoryItem>().GetFirstOrDefaultAsync(predicate: x => x.AppraisalCategoryItemID == item.AppraisalCategoryItemID);

                    var category = await _unitOfWork.GetRepository<AppraisalCategory>().GetFirstOrDefaultAsync(predicate: x => x.AppraisalCategoryCode.ToLower() == item.AppraisalCategoryCode.ToLower());

                    if (check == null)
                    {
                        var appraisalCategoryItem = new AppraisalCategoryItem() { AppraisalCategoryCode = item.AppraisalCategoryCode, AppraisalCategoryId = (category != null) ? category.Id : Guid.Empty, Description = item.Desccription, Weight = item.Weight, AppraisalCategoryItemID = item.AppraisalCategoryItemID, StaffType = item.TypeCode, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };

                        _unitOfWork.GetRepository<AppraisalCategoryItem>().Insert(appraisalCategoryItem);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }
    }
}
