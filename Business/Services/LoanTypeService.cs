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
    public class LoanTypeService: ILoanTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public LoanTypeService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:PRServerConnection"]);
        }

        public async Task<BaseResponse> Create(LoanType model)
        {
            var check = await _unitOfWork.GetRepository<LoanType>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower() == model.Name.ToLower());
            if (check == null)
            {
                _unitOfWork.GetRepository<LoanType>().Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.RecordExist };
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var model = await GetById(id);
            if (model != null)
            {
                _unitOfWork.GetRepository<LoanType>().Delete(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<LoanType>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<BaseResponse> Edit(Guid id, string name)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.Name = name;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<LoanType>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<LoanType>> GetAll(Expression<Func<LoanType, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<LoanType>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<LoanType> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<LoanType>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }

        public async Task<BaseResponse> RefreshLoanType()
        {
            var sql = "select * from PRZ_LNTYPE";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<PRZ_LNTYPE> resource = new List<PRZ_LNTYPE>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    PRZ_LNTYPE requester = new PRZ_LNTYPE()
                    {
                        prz_lntypeCode = reader["prz_lntypeCode"].ToString(),
                        int_code = reader["int_code"].ToString(),
                        short_desc = reader["short_desc"].ToString(),
                        long_desc = reader["long_desc"].ToString()
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<LoanType>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower() == item.long_desc.ToLower());

                    if (check == null)
                    {
                        var loanType = new LoanType() { Name = item.long_desc, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };

                        _unitOfWork.GetRepository<LoanType>().Insert(loanType);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }
    }
}
