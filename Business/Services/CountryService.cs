﻿using Business.Interfaces;
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
    public class CountryService: ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;

        public CountryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(HRDbConfig.ConnectionStringUrl);
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HRCountry";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRCountry> resource = new List<HRCountry>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRCountry requester = new HRCountry()
                    {
                        CountryCode = reader["CountryCode"].ToString(),
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
                    var check = await _unitOfWork.GetRepository<Country>().GetFirstOrDefaultAsync(predicate: x => x.CountryCode.ToLower() == item.CountryCode.ToLower());

                    if (check == null)
                    {
                        var Country = new Country() { CountryCode = item.CountryCode, Descc = item.descc, CreatedDate = DateTime.Now, Id = Guid.NewGuid()};

                        _unitOfWork.GetRepository<Country>().Insert(Country);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<Country>> GetAll(Expression<Func<Country, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Country>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<IEnumerable<Country>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }
    }
}