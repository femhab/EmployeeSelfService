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
    public class CourtesyService: ICourtesyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;

        public CourtesyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(HRDbConfig.ConnectionStringUrl);
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HRCourtesy";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRCourtesy> resource = new List<HRCourtesy>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRCourtesy requester = new HRCourtesy()
                    {
                        CourtesyCode = reader["CourtesyCode"].ToString(),
                        descc = reader["descc"].ToString()
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<Courtesy>().GetFirstOrDefaultAsync(predicate: x => x.CourtesyCode.ToLower() == item.CourtesyCode.ToLower());

                    if (check == null)
                    {
                        var Courtesy = new Courtesy() { CourtesyCode = item.CourtesyCode, Descc = item.descc, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };

                        _unitOfWork.GetRepository<Courtesy>().Insert(Courtesy);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<Courtesy>> GetAll(Expression<Func<Courtesy, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Courtesy>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<IEnumerable<Courtesy>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }
    }
}
