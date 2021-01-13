using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class EmployeeEducationalDetailService : IEmployeeEducationalDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public EmployeeEducationalDetailService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
        }

        public async  Task<BaseResponse> Create(EmployeeEducationDetail model)
        {
            var check = await GetAll(x => x.EmployeeId == model.EmployeeId && x.EducationalQualificationId == model.EducationalQualificationId);
            if (check != null || check.Count() < 2)
            {
                _unitOfWork.GetRepository<EmployeeEducationDetail>().Insert(model);
                await _unitOfWork.SaveChangesAsync();
                var insert = await _unitOfWork.GetRepository<EmployeeEducationDetail>().GetFirstOrDefaultAsync(predicate: x => x.Id == model.Id, null, include: c => c.Include(i => i.EducationalLevel).Include(i => i.EducationalGrade).Include(i => i.EducationalQualification));
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = _sqlConnection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"INSERT INTO TempHREmpEdu(Emp_No,EduclLevelCode,EduTypCode,SchoolCode,grad_year,Note,DegreeCode,EducDspCode,GradeCode,CountryCode,Insertusername,InsertTransacDate,InsertTransacType) 
                                VALUES(@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14)";

                        //cmd.Parameters.AddWithValue("@param1", 0);
                        cmd.Parameters.AddWithValue("@param2", insert.Emp_No);
                        cmd.Parameters.AddWithValue("@param3", insert.EducationalLevel.Code);
                        cmd.Parameters.AddWithValue("@param4", DBNull.Value);
                        cmd.Parameters.AddWithValue("@param5", DBNull.Value);
                        cmd.Parameters.AddWithValue("@param6", insert.EndDate);
                        cmd.Parameters.AddWithValue("@param7", DBNull.Value);
                        cmd.Parameters.AddWithValue("@param8", insert.EducationalQualification.Code);
                        cmd.Parameters.AddWithValue("@param9", DBNull.Value);
                        cmd.Parameters.AddWithValue("@param10", insert.EducationalGrade.Code);
                        cmd.Parameters.AddWithValue("@param11", DBNull.Value);
                        cmd.Parameters.AddWithValue("@param12", "Employee");
                        cmd.Parameters.AddWithValue("@param13", DateTime.Now);
                        cmd.Parameters.AddWithValue("@param14", "Insert");


                        _sqlConnection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }

                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.MaximumReached };
        }

        public async Task<BaseResponse> Edit(Guid id, Guid levelId, Guid qualificationId, Guid gradeId)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.EducationalLevelId = levelId;
                model.EducationalQualificationId = qualificationId;
                model.EducationalGradeId = gradeId;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<EmployeeEducationDetail>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<EmployeeEducationDetail>> GetByEmployee(Guid employeeId)
        {
            var data = await GetAll(x => x.EmployeeId == employeeId, "Employee,EducationalLevel,EducationalQualification,EducationalGrade");
            return data;
        }

        public async Task<IEnumerable<EmployeeEducationDetail>> GetAll(Expression<Func<EmployeeEducationDetail, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EmployeeEducationDetail>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<EmployeeEducationDetail> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<EmployeeEducationDetail>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
