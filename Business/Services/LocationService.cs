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
    public class LocationService: ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;

        public LocationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(HRDbConfig.ConnectionStringUrl);
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HRLocation";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRLocation> resource = new List<HRLocation>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRLocation requester = new HRLocation()
                    {
                        LocationCode = reader["LocationCode"].ToString(),
                        descc = reader["descc"].ToString(),
                        StateCode = reader["StateCode"].ToString(),
                        AccountNo = reader["AccountNo"].ToString()
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<Location>().GetFirstOrDefaultAsync(predicate: x => x.LocationCode.ToLower() == item.LocationCode.ToLower());

                    if (check == null)
                    {
                        var Location = new Location() { LocationCode = item.LocationCode, Descc = item.descc, CreatedDate = DateTime.Now, Id = Guid.NewGuid(), AccountNo = item.AccountNo, StateCode = item.StateCode };

                        _unitOfWork.GetRepository<Location>().Insert(Location);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<Location>> GetAll(Expression<Func<Location, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Location>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<IEnumerable<Location>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }
    }
}
