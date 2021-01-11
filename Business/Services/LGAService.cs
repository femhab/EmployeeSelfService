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
    public class LGAService: ILGAService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public LGAService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HRLGA";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRLGA> resource = new List<HRLGA>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRLGA requester = new HRLGA()
                    {
                        LGACode = reader["LGACode"].ToString(),
                        descc = reader["descc"].ToString(),
                        StateCode = reader["StateCode"].ToString()
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<LGA>().GetFirstOrDefaultAsync(predicate: x => x.LGACode.ToLower() == item.LGACode.ToLower());

                    if (check == null)
                    {
                        var LGA = new LGA() { LGACode = item.LGACode, Descc = item.descc, CreatedDate = DateTime.Now, Id = Guid.NewGuid(), StateCode = item.StateCode };

                        _unitOfWork.GetRepository<LGA>().Insert(LGA);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<LGA>> GetAll(Expression<Func<LGA, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<LGA>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<IEnumerable<LGA>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }
    }
}
