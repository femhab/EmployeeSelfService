using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Services
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ServiceContext _dbContext;
        private readonly IApprovalBoardService _approvalBoardService;

        public EmployeeService(IUnitOfWork unitOfWork, ServiceContext dbContext, IApprovalBoardService approvalBoardService)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _approvalBoardService = approvalBoardService;
        }

        public async Task<BaseResponse> Create(Employee model)
        {
            var check = await _unitOfWork.GetRepository<Employee>().GetFirstOrDefaultAsync(predicate: x => x.Emp_No == model.Emp_No);
            if (check == null)
            {
                //update some data from hr db
                var hrData = await _dbContext.HREmpMst.FirstOrDefaultAsync(x => x.Emp_No == model.Emp_No);
                if(hrData != null)
                {
                    var department = await _unitOfWork.GetRepository<Department>().GetFirstOrDefaultAsync(predicate: x => x.DeptCode.ToLower() == hrData.DeptCode.ToLower());
                    var division = await _unitOfWork.GetRepository<Division>().GetFirstOrDefaultAsync(predicate: x => x.DivisonCode.ToLower() == hrData.DivisionCode.ToLower());
                    var unit = await _unitOfWork.GetRepository<Unit>().GetFirstOrDefaultAsync(predicate: x => x.UnitCode.ToLower() == hrData.UnitCode.ToLower());
                    var gradeLevel = await _unitOfWork.GetRepository<GradeLevel>().GetFirstOrDefaultAsync(predicate: x => x.GradeCode.ToLower() == hrData.GradeCode.ToLower());

                    model.DepartmentId = department.Id;
                    model.DivisionId = division.Id;
                    model.UnitId = unit.Id;
                    model.GradeLevelId = gradeLevel.Id;
                    model.Status = Status.Active;
                    model.AccessType = AccessType.Employee;
                    model.DOB = hrData.date_birth;
                    model.EmploymentDate = hrData.date_Emp;
                }
                _unitOfWork.GetRepository<Employee>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                //register the user in identity table

                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.RecordExist };
        }

        public async Task<BaseResponse> RequestBasicInfoChange(Guid id, string lastName, string firstName, string email)
        {
            var model = await GetById(id);
            if (model != null)
            {
                var appliedUpdate = new AppliedNameUpdate()
                {
                    EmployeeId = id,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = email,
                    Emp_No = model.Emp_No

                };
                
                _unitOfWork.GetRepository<AppliedNameUpdate>().Update(appliedUpdate);
                await _unitOfWork.SaveChangesAsync();

                //submit for approval
                var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("information"));
                var approvalProcessor = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalLevel == Level.HR);

                await _approvalBoardService.Create(new ApprovalBoard()
                {
                    EmployeeId = model.Id,
                    ApprovalLevel = Level.HR,
                    Emp_No = model.Emp_No,
                    ApprovalWorkItemId = approvalWorkItem.Id,
                    ApprovalProcessorId = approvalProcessor.Id,
                    ServiceId = model.Id,
                    Status = ApprovalStatus.Pending,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    CreatedBy = model.Emp_No
                });

                return new BaseResponse() { Status = true, Message = ResponseMessage.AwaitingApproval }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> RequestTransfer(Guid id, Guid divisionId, Guid departmentId, Guid unitId)
        {
            var model = await GetById(id);
            if (model != null)
            {
                if(model.DepartmentId != departmentId)
                {
                    var appliedTransfer = new AppliedTransfer()
                    {
                        EmployeeId = id,
                        DivisionId = divisionId,
                        DepartmentId = departmentId,
                        UnitId = unitId,
                        Emp_No = model.Emp_No

                    };
                    _unitOfWork.GetRepository<AppliedTransfer>().Update(appliedTransfer);
                    await _unitOfWork.SaveChangesAsync();

                    //submit for approval
                    var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("information"));
                    var approvalProcessor = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalLevel == Level.HR);

                    await _approvalBoardService.Create(new ApprovalBoard()
                    {
                        EmployeeId = model.Id,
                        ApprovalLevel = Level.HR,
                        Emp_No = model.Emp_No,
                        ApprovalWorkItemId = approvalWorkItem.Id,
                        ApprovalProcessorId = approvalProcessor.Id,
                        ServiceId = model.Id,
                        Status = ApprovalStatus.Pending,
                        CreatedDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        CreatedBy = model.Emp_No
                    });

                    return new BaseResponse() { Status = true, Message = ResponseMessage.AwaitingApproval };
                }
                else
                {
                    model.DivisionId = divisionId;
                    model.DepartmentId = departmentId;
                    model.UnitId = unitId;
                    _unitOfWork.GetRepository<Employee>().Update(model);
                    await _unitOfWork.SaveChangesAsync();
                    return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
                }    
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()), "Employee,Department,Division,Unit,GradeLevel");
            return data;
        }

        public async Task<IEnumerable<Employee>> GetByDepartment(Guid departmentId)
        {
            var data = await GetAll(x => x.DepartmentId != departmentId, "Employee,Department,Division,Unit,GradeLevel");
            return data;
        }

        public async Task<Employee> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<Employee>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }

        public async Task<IEnumerable<HRUsers>> GetUnregisteredBaseEmployee()
        {
            var hrData = await _dbContext.HRUsers.ToListAsync();
            var registeredEmployee = await GetAll();

            List<HRUsers> unRegisteredUser = new List<HRUsers>();

            foreach(var item in hrData)
            {
                if (registeredEmployee.Select(x => x.Emp_No == item.Emp_No).Count() == 0)
                    unRegisteredUser.Add(item);
            }
            return unRegisteredUser;
        }

        public async Task<IEnumerable<Employee>> GetAll(Expression<Func<Employee, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Employee>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }
    }
}
