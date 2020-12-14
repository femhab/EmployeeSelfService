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
    public class AvailabilityStatusService: IAvailabilityStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;

        public AvailabilityStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(HRDbConfig.ConnectionStringUrl);
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HRStatus";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRStatus> resource = new List<HRStatus>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRStatus requester = new HRStatus()
                    {
                        StatusCode = reader["StatusCode"].ToString(),
                        descc = reader["descc"].ToString(),
                        active = reader["active"].ToString()
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<AvalaibilityStatus>().GetFirstOrDefaultAsync(predicate: x => x.StatusCode.ToLower() == item.StatusCode.ToLower());

                    if (check == null)
                    {
                        var availabilityStatus = new AvalaibilityStatus() { StatusCode = item.StatusCode, Descc = item.descc, Active = item.active, CreatedDate = DateTime.Now, Id = Guid.NewGuid()};

                        _unitOfWork.GetRepository<AvalaibilityStatus>().Insert(availabilityStatus);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<AvalaibilityStatus>> GetAll(Expression<Func<AvalaibilityStatus, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<AvalaibilityStatus>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<IEnumerable<AvalaibilityStatus>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }
    }
}
