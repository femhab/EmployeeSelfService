using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class EmployeeNOKDetailService: IEmployeeNOKDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly INotificationService _notificationService;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDocumentService _documentService;

        public EmployeeNOKDetailService(IUnitOfWork unitOfWork, IApprovalBoardService approvalBoardService, INotificationService notificationService, IConfiguration configuration, IHostingEnvironment hostingEnvironment, IDocumentService documentService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _approvalBoardService = approvalBoardService;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
            _notificationService = notificationService;
            _hostingEnvironment = hostingEnvironment;
            _documentService = documentService;
        }

        public async Task<BaseResponse> Create(EmployeeNOKDetail model)
        {
            var checkNok = await GetAll(x => x.EmployeeId == model.EmployeeId && !x.IsEmergencyContact);
            var checkEmergencyContact = await GetAll(x => x.EmployeeId == model.EmployeeId && x.IsEmergencyContact);

            if ( checkNok.Count() > 2)
            {
                return new BaseResponse() { Status = false, Message = ResponseMessage.MaxNokReached };
            }

            if (checkEmergencyContact.Count() > 2)
            {
                return new BaseResponse() { Status = false, Message = ResponseMessage.MaxEmerReached };
            }

            _unitOfWork.GetRepository<EmployeeNOKDetail>().Insert(model);
            await _unitOfWork.SaveChangesAsync();

            //submit for approval
            var insert = await _unitOfWork.GetRepository<EmployeeNOKDetail>().GetFirstOrDefaultAsync(predicate: x => x.Id == model.Id, null, include: c => c.Include(i => i.Employee).Include(i => i.Relationship));

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = _sqlConnection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO TempHREmpNextOfKin(NOKName,NOKPhone,NOKAddress,NOKEmail,emp_no,DepCode,Insertusername,InsertTransacDate,InsertTransacType) 
                                VALUES(@param2,@param3,@param4,@param5,@param6,@param8,@param9,@param10,@param11)";
                    cmd.Parameters.AddWithValue("@param2", insert.LastName + " " + insert.FirstName);
                    cmd.Parameters.AddWithValue("@param3", insert.PhoneNumber);
                    cmd.Parameters.AddWithValue("@param4", insert.Address);
                    cmd.Parameters.AddWithValue("@param5", insert.Email);
                    cmd.Parameters.AddWithValue("@param6", insert.Emp_No);
                    //cmd.Parameters.AddWithValue("@param7", null);
                    cmd.Parameters.AddWithValue("@param8", insert.Relationship.Code);
                    cmd.Parameters.AddWithValue("@param9", "Employee");
                    cmd.Parameters.AddWithValue("@param10", DateTime.Now);
                    cmd.Parameters.AddWithValue("@param11", "Insert");


                    _sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            await _notificationService.CreateNotification(NotificationAction.NOKCreateTitle, NotificationAction.NOKCreateMessage, model.EmployeeId, false, false);

            return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var model = await GetById(id);
            if (model != null)
            {
                _unitOfWork.GetRepository<EmployeeNOKDetail>().Delete(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Edit(Guid id, string firstName, string lastName, string email, string phoneNumber, DateTime? dob, string address, Guid relationshipId)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.FirstName = firstName;
                model.LastName = lastName;
                model.Email = email;
                model.PhoneNumber = phoneNumber;
                model.DOB = dob;
                model.Address = address;
                model.RelationshipId = relationshipId;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<EmployeeNOKDetail>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful } ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<EmployeeNOKDetail>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()), "Relationship,Employee");
            return data;
        }

        public async Task<IEnumerable<EmployeeNOKDetail>> GetByEmployee(Guid employeeId)
        {
            var data = await GetAll(x => x.EmployeeId == employeeId, "Relationship");
            return data;
        }

        public async Task<IEnumerable<EmployeeNOKDetail>> GetAll(Expression<Func<EmployeeNOKDetail, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EmployeeNOKDetail>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<EmployeeNOKDetail> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<EmployeeNOKDetail>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
