using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class EmployeeFamilyDependentService: IEmployeeFamilyDependentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly INotificationService _notificationService;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public EmployeeFamilyDependentService(IUnitOfWork unitOfWork, IApprovalBoardService approvalBoardService, INotificationService notificationService, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _approvalBoardService = approvalBoardService;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
            _notificationService = notificationService;
        }

        public async Task<BaseResponse> Create(EmployeeFamilyDependent model)
        {
            var check = await GetAll(x => x.EmployeeId == model.EmployeeId);
            if (check.Count() < 3)
            {
                _unitOfWork.GetRepository<EmployeeFamilyDependent>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                //submit for approval
                var insert = await _unitOfWork.GetRepository<EmployeeFamilyDependent>().GetFirstOrDefaultAsync(predicate: x => x.Id == model.Id, null, include: c => c.Include(i => i.Employee).Include(i => i.Relationship));

                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = _sqlConnection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"INSERT INTO HREmpDepD(Emp_No,first_Name,last_Name,date_birth,DepCode,GenoTypeCode,BloodCode,active,gender,Insertusername,InsertTransacDate,InsertTransacType) 
                                VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param11,@param12,@param13)";
                        cmd.Parameters.AddWithValue("@param1", insert.Emp_No);
                        cmd.Parameters.AddWithValue("@param2", insert.FirstName);
                        cmd.Parameters.AddWithValue("@param3", insert.LastName);
                        cmd.Parameters.AddWithValue("@param4", insert.DOB);
                        cmd.Parameters.AddWithValue("@param5", insert.Relationship.Code);
                        cmd.Parameters.AddWithValue("@param6", DBNull.Value);
                        cmd.Parameters.AddWithValue("@param7", DBNull.Value);
                        cmd.Parameters.AddWithValue("@param8", "True");
                        cmd.Parameters.AddWithValue("@param9", DBNull.Value);
                        //cmd.Parameters.AddWithValue("@param10", insert.Relationship.Code);
                        cmd.Parameters.AddWithValue("@param11", "Employee");
                        cmd.Parameters.AddWithValue("@param12", DateTime.Now);
                        cmd.Parameters.AddWithValue("@param13", "Insert");


                        _sqlConnection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }

                await _notificationService.CreateNotification(NotificationAction.DependentCreateTitle, NotificationAction.DependentCreateMessage, model.EmployeeId, false, false);

                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.MaximumReached };
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var model = await GetById(id);
            if (model != null)
            {
                _unitOfWork.GetRepository<EmployeeFamilyDependent>().Delete(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Edit(Guid id, string firstName, string lastName, string phoneNumber, DateTime? dob, string address, Guid relationshipId)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.FirstName = firstName;
                model.LastName = lastName;
                model.PhoneNumber = phoneNumber;
                model.DOB = dob;
                model.Address = address;
                model.RelationshipId = relationshipId;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<EmployeeFamilyDependent>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<EmployeeFamilyDependent>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<IEnumerable<EmployeeFamilyDependent>> GetByEmployee(Guid employeeId)
        {
            var data = await GetAll(x => x.EmployeeId == employeeId, "Relationship");
            return data;
        }

        public async Task<IEnumerable<EmployeeFamilyDependent>> GetAll(Expression<Func<EmployeeFamilyDependent, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EmployeeFamilyDependent>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<EmployeeFamilyDependent> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<EmployeeFamilyDependent>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
