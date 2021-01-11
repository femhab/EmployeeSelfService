using System;
using System.Collections.Generic;
using System.Data;
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

namespace Business.Services
{
    public class DisciplinaryActionService : IDisciplinaryActionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _configuration;

        public DisciplinaryActionService(IUnitOfWork unitOfWork, INotificationService notificationService, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
            _notificationService = notificationService;
        }

        public async Task<BaseResponse> CreateQuery(Guid employeeId, string empNo, string subject, string message, Guid targetEmployeeId, string targetEmployeeNo)
        {
            if (!string.IsNullOrEmpty(subject) || !string.IsNullOrEmpty(message))
            {
                var model = new DisciplinaryAction()
                {
                    EmployeeId = employeeId,
                    Emp_No = empNo,
                    QuerySubject = subject,
                    QueryMessage = message.Replace("<p>",string.Empty).Replace("</p>", string.Empty).Replace("<br>", string.Empty).Replace("</br>", string.Empty),
                    TargetEmployeeId = targetEmployeeId,
                    TargetEmployeeNo = targetEmployeeNo,
                    Action = QueryAction.Pending,
                    QueryDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid()
                };

                _unitOfWork.GetRepository<DisciplinaryAction>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                await _notificationService.CreateNotification(NotificationAction.DisciplinaryCreateTitle, NotificationAction.DisciplinaryCreateMessage, model.EmployeeId, false, false);

                return new BaseResponse() { Status = true, Message = ResponseMessage.QueryCreatedSuccessfully };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<DisciplinaryAction>> GetByEmployee(Guid employeeId)
        {
            var data = await GetAll(x => x.EmployeeId == employeeId, "Employee");
            return data;
        }

        public async Task<IEnumerable<DisciplinaryAction>> GetByTargetEmployee(Guid targetEmployeeId)
        {
            var data = await GetAll(x => x.TargetEmployeeId == targetEmployeeId, "Employee");
            return data;
        }

        public async Task<BaseResponse> GiveAction(Guid queryId, string actionComment, QueryAction action)
        {
            var check = await _unitOfWork.GetRepository<DisciplinaryAction>().GetFirstOrDefaultAsync(predicate: x => x.Id == queryId);
            if (check != null)
            {
                check.QueryActionComment = actionComment.Replace("<p>", string.Empty).Replace("</p>", string.Empty).Replace("<br>", string.Empty).Replace("</br>", string.Empty);
                check.QueryActionDate = DateTime.Now;
                check.Action = action;
                check.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<DisciplinaryAction>().Update(check);
                await _unitOfWork.SaveChangesAsync();

                //using (SqlCommand cmd = new SqlCommand())
                //{
                //    try
                //    {
                //        cmd.Connection = _sqlConnection;
                //        cmd.CommandType = CommandType.Text;
                //        cmd.CommandText = @"INSERT INTO HREmpEdu(Emp_No,EduclLevelCode,EduTypCode,SchoolCode,grad_year,Note,DegreeCode,EducDspCode,GradeCode,CountryCode,Insertusername,InsertTransacDate,InsertTransacType) 
                //                VALUES(@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14)";

                //        //cmd.Parameters.AddWithValue("@param1", 0);
                //        cmd.Parameters.AddWithValue("@param2", insert.Emp_No);
                //        cmd.Parameters.AddWithValue("@param3", insert.EducationalLevel.Code);
                //        cmd.Parameters.AddWithValue("@param4", DBNull.Value);
                //        cmd.Parameters.AddWithValue("@param5", DBNull.Value);
                //        cmd.Parameters.AddWithValue("@param6", insert.EndDate);
                //        cmd.Parameters.AddWithValue("@param7", DBNull.Value);
                //        cmd.Parameters.AddWithValue("@param8", insert.EducationalQualification.Code);
                //        cmd.Parameters.AddWithValue("@param9", DBNull.Value);
                //        cmd.Parameters.AddWithValue("@param10", insert.EducationalGrade.Code);
                //        cmd.Parameters.AddWithValue("@param11", DBNull.Value);
                //        cmd.Parameters.AddWithValue("@param12", "Employee");
                //        cmd.Parameters.AddWithValue("@param13", DateTime.Now);
                //        cmd.Parameters.AddWithValue("@param14", "Insert");


                //        _sqlConnection.Open();
                //        cmd.ExecuteNonQuery();
                //    }
                //    catch (Exception ex)
                //    {
                //        throw ex;
                //    }

                //}

                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.NoRecordExist };
        }

        public async Task<BaseResponse> ReplyQuery(Guid queryId, string reply)
        {
            var check = await _unitOfWork.GetRepository<DisciplinaryAction>().GetFirstOrDefaultAsync(predicate: x => x.Id == queryId);
            if (check != null)
            {
                check.QueryReply = reply.Replace("<p>", string.Empty).Replace("</p>", string.Empty).Replace("<br>", string.Empty).Replace("</br>", string.Empty);
                check.Action = QueryAction.Replied;
                check.QueryReplyDate = DateTime.Now;
                check.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<DisciplinaryAction>().Update(check);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.NoRecordExist };
        }

        public async Task<IEnumerable<DisciplinaryAction>> GetAll(Expression<Func<DisciplinaryAction, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<DisciplinaryAction>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<DisciplinaryAction> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<DisciplinaryAction>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
