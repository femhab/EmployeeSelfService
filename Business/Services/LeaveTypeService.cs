using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Services
{
    public class LeaveTypeService: ILeaveTypeService
    {
        private readonly ServiceContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public LeaveTypeService(ServiceContext dbContext, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
        }

        public async Task<BaseResponse> Edit(Guid id, int availableDays)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.AvailableDays = availableDays;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<LeaveType>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<LeaveType>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<BaseResponse> Refresh()
        {
            //var hrData = await _dbContext.hrleavedays.ToListAsync();
            var sql = "select * from hrleavedays";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<hrleavedays> resource = new List<hrleavedays>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    hrleavedays requester = new hrleavedays()
                    {
                        gradeCode = reader["gradeCode"].ToString(),
                        leaveDays = reader.GetInt32(reader.GetOrdinal("leaveDays"))
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var gradeLevel = await _unitOfWork.GetRepository<GradeLevel>().GetFirstOrDefaultAsync(predicate: x => x.GradeCode.ToLower() == item.gradeCode.ToLower());
                    //for annual
                    if(gradeLevel != null)
                    {
                        List<LeaveType> leaveTypeList = new List<LeaveType>(); 
                        var model = new LeaveType() { GradeLevelId = gradeLevel.Id, AvailableDays = item.leaveDays, Class = LeaveClass.Annual, Id = Guid.NewGuid(), CreatedDate = DateTime.Now };
                        //_unitOfWork.GetRepository<LeaveType>().Insert(model);
                        leaveTypeList.Add(model);
                        //for others

                        leaveTypeList.Add(new LeaveType() { GradeLevelId = gradeLevel.Id, AvailableDays = 0, Class = LeaveClass.Casual, Id = Guid.NewGuid(), CreatedDate = DateTime.Now});

                        leaveTypeList.Add(new LeaveType() { GradeLevelId = gradeLevel.Id, AvailableDays = 0, Class = LeaveClass.Exam, Id = Guid.NewGuid(), CreatedDate = DateTime.Now });
                        //
                        leaveTypeList.Add(new LeaveType() { GradeLevelId = gradeLevel.Id, AvailableDays = 0, Class = LeaveClass.Maternity, Id = Guid.NewGuid(), CreatedDate = DateTime.Now});
                        //
                        leaveTypeList.Add(new LeaveType() { GradeLevelId = gradeLevel.Id, AvailableDays = 0, Class = LeaveClass.Sick, Id = Guid.NewGuid(), CreatedDate = DateTime.Now });
                        //
                        leaveTypeList.Add(new LeaveType() { GradeLevelId = gradeLevel.Id, AvailableDays = 0, Class = LeaveClass.Study, Id = Guid.NewGuid(), CreatedDate = DateTime.Now });

                        foreach(var leaveTypes in leaveTypeList)
                        {
                            _unitOfWork.GetRepository<LeaveType>().Insert(leaveTypes);
                        }
                    }
                    
                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<LeaveType>> GetAll(Expression<Func<LeaveType, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<LeaveType>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), "GradeLevel");
            return model;
        }

        public async Task<LeaveType> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<LeaveType>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
