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
    public class EmployeeTitleService: IEmployeeTitleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public EmployeeTitleService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HRJTitle";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRJTitle> resource = new List<HRJTitle>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRJTitle requester = new HRJTitle()
                    {
                        TitleCode = reader["TitleCode"].ToString(),
                        descc = reader["descc"].ToString(),
                        slot = reader.GetInt32(reader.GetOrdinal("slot"))
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<EmployeeTitle>().GetFirstOrDefaultAsync(predicate: x => x.TitleCode.ToLower() == item.TitleCode.ToLower());

                    if (check == null)
                    {
                        var EmployeeTitle = new EmployeeTitle() { TitleCode = item.TitleCode, Descc = item.descc, Slot = item.slot, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };

                        _unitOfWork.GetRepository<EmployeeTitle>().Insert(EmployeeTitle);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<EmployeeTitle>> GetAll(Expression<Func<EmployeeTitle, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EmployeeTitle>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<IEnumerable<EmployeeTitle>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }
    }
}
