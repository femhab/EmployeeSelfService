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
    public class StateService: IStateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;

        public StateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(HRDbConfig.ConnectionStringUrl);
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HRState";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRState> resource = new List<HRState>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRState requester = new HRState()
                    {
                        StateCode = reader["StateCode"].ToString(),
                        descc = reader["descc"].ToString(),
                        ZoneCode = reader["ZoneCode"].ToString()
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<State>().GetFirstOrDefaultAsync(predicate: x => x.StateCode.ToLower() == item.StateCode.ToLower());

                    if (check == null)
                    {
                        var State = new State() { StateCode = item.StateCode, Descc = item.descc, ZoneCode = item.ZoneCode, CreatedDate = DateTime.Now, Id = Guid.NewGuid()};

                        _unitOfWork.GetRepository<State>().Insert(State);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<State>> GetAll(Expression<Func<State, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<State>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<IEnumerable<State>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }
    }
}
