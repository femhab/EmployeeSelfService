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
    public class SectionService: ISectionService
    {
        private readonly ServiceContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public SectionService(ServiceContext dbContext, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
        }

        public async Task<BaseResponse> Refresh()
        {
            var sql = "select * from HRSection";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRSection> resource = new List<HRSection>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRSection requester = new HRSection()
                    {
                        SectionCode = reader["SectionCode"].ToString(),
                        DeptCode = reader["DeptCode"].ToString(),
                        CompanyCode = reader["CompanyCode"].ToString(),
                        DivisionCode = reader["DivisionCode"].ToString(),
                        descc = reader["descc"].ToString(),
                        section_acct = reader["section_acct"].ToString(),
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<Section>().GetFirstOrDefaultAsync(predicate: x => x.SectionCode.ToLower() == item.SectionCode.ToLower());

                    if (check == null)
                    {
                        var section = new Section() { SectionCode = item.SectionCode, DeptCode = item.DeptCode, DivisionCode = item.DivisionCode, Descc = item.descc, CompanyCode = item.CompanyCode, CreatedDate = DateTime.Now, Id = Guid.NewGuid(), SectionAccount = item.section_acct };

                        _unitOfWork.GetRepository<Section>().Insert(section);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<Section>> GetAll(Expression<Func<Section, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Section>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id),include);
            return model;
        }

        public async Task<IEnumerable<Section>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }
    }
}
