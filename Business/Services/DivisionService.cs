using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Services
{
    public class DivisionService: IDivisionService
    {
        private readonly ServiceContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;

        public DivisionService(ServiceContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(HRDbConfig.ConnectionStringUrl);
        }

        public async Task<IEnumerable<Division>> GetAll(Expression<Func<Division, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Division>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<IEnumerable<Division>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HRDivision";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRDivision> resource = new List<HRDivision>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRDivision requester = new HRDivision()
                    {
                        CompanyCode = reader["CompanyCode"].ToString(),
                        DivisionCode = reader["DivisionCode"].ToString(),
                        DESCC = reader["DESCC"].ToString()
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<Division>().GetFirstOrDefaultAsync(predicate: x => x.DivisonCode.ToLower() == item.DivisionCode.ToLower());

                    if (check == null)
                    {
                        var division = new Division() { DivisonCode = item.DivisionCode, Descc = item.DESCC, CompanyCode = item.CompanyCode, CreatedDate = DateTime.Now, Id = Guid.NewGuid()};

                        _unitOfWork.GetRepository<Division>().Insert(division);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }
    }
}
