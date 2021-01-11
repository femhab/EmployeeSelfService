using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ViewModel.Model;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Services
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ServiceContext _dbContext;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IGradeLevelService _gradeLevelService;
        private readonly IApprovalBoardActiveLevelService _approvalBoardActiveLevelService;
        private readonly SqlConnection _sqlConnection;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public EmployeeService(IUnitOfWork unitOfWork, ServiceContext dbContext, IApprovalBoardService approvalBoardService, IMapper mapper, IGradeLevelService gradeLevelService, IApprovalBoardActiveLevelService approvalBoardActiveLevelService, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _approvalBoardService = approvalBoardService;
            _gradeLevelService = gradeLevelService;
            _approvalBoardActiveLevelService = approvalBoardActiveLevelService;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
            _mapper = mapper;
        }

        public async Task<EmployeeResponseModel> Create(Employee model)
        {
            var check = await _unitOfWork.GetRepository<Employee>().GetFirstOrDefaultAsync(predicate: x => x.Emp_No == model.Emp_No);
            if (check == null)
            {
                //update some data from hr db
                //var hrData = await _dbContext.HREmpMst.FirstOrDefaultAsync(x => x.Emp_No == model.Emp_No);
                var sql = $"select * from HREmpMst where Emp_No='{model.Emp_No}'";
                SqlCommand query = new SqlCommand(sql, _sqlConnection);
                HREmpMst hrData = new HREmpMst() { };
                _sqlConnection.Open();
                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            HREmpMst requester = new HREmpMst()
                            {
                                DepCode = reader["DepCode"].ToString(),
                                DeptCode = reader["DeptCode"].ToString(),
                                DivisionCode = reader["DivisionCode"].ToString(),
                                UnitCode = reader["UnitCode"].ToString(),
                                GradeCode = reader["GradeCode"].ToString(),
                                Emp_No = reader["Emp_No"].ToString(),
                                TypeCode = reader["TypeCode"].ToString(),
                                SectionCode = reader["SectionCode"].ToString(),
                                TitleCode = reader["TitleCode"].ToString(),
                                MaritalCode = reader["MaritalCode"].ToString(),
                                StatusCode = reader["StatusCode"].ToString(),
                                CountryCode = reader["CountryCode"].ToString(),
                                StateCode = reader["StateCode"].ToString(),
                                LGACode = reader["LGACode"].ToString(),
                                LocationCode = reader["LocationCode"].ToString(),
                                CourtesyCode = reader["CourtesyCode"].ToString(),
                                date_Emp = Convert.ToDateTime(reader["date_Emp"]).ToString("dd/MM/yyyy"),
                                date_birth = Convert.ToDateTime(reader["date_birth"]).ToString("dd/MM/yyyy"),
                                date_conf = Convert.ToDateTime(reader["date_conf"]).ToString("dd/MM/yyyy"),
                                effectiveDate = Convert.ToDateTime(reader["effectiveDate"]).ToString("dd/MM/yyyy"),
                                preAppDate = Convert.ToDateTime(reader["preAppDate"]).ToString("dd/MM/yyyy"),
                                projRetireDate = Convert.ToDateTime(reader["projRetireDate"]).ToString("dd/MM/yyyy"),
                            };
                            hrData = requester;
                        }
                        catch(Exception ex)
                        {
                            throw ex;
                        }
                    }
                    _sqlConnection.Close();
                }

                if (hrData.Emp_No != null)
                {
                    var department = await _unitOfWork.GetRepository<Department>().GetFirstOrDefaultAsync(predicate: x => x.DeptCode.ToLower() == hrData.DeptCode.ToLower()) ?? null;
                    var division = await _unitOfWork.GetRepository<Division>().GetFirstOrDefaultAsync(predicate: x => x.DivisonCode.ToLower() == hrData.DivisionCode.ToLower()) ?? null;
                    var unit = await _unitOfWork.GetRepository<Unit>().GetFirstOrDefaultAsync(predicate: x => x.UnitCode.ToLower() == hrData.UnitCode.ToLower()) ?? null;
                    var gradeLevel = await _unitOfWork.GetRepository<GradeLevel>().GetFirstOrDefaultAsync(predicate: x => x.GradeCode.ToLower() == hrData.GradeCode.ToLower()) ?? null;
                    var section = await _unitOfWork.GetRepository<Section>().GetFirstOrDefaultAsync(predicate: x => x.SectionCode.ToLower() == hrData.SectionCode.ToLower()) ?? null;
                    var title = await _unitOfWork.GetRepository<EmployeeTitle>().GetFirstOrDefaultAsync(predicate: x => x.TitleCode.ToLower() == hrData.TitleCode.ToLower()) ?? null;
                    var maritalStatus = await _unitOfWork.GetRepository<MaritalStatus>().GetFirstOrDefaultAsync(predicate: x => x.MaritalCode.ToLower() == hrData.MaritalCode.ToLower())?? null;
                    var availability = await _unitOfWork.GetRepository<AvalaibilityStatus>().GetFirstOrDefaultAsync(predicate: x => x.StatusCode.ToLower() == hrData.StatusCode.ToLower()) ?? null;
                    var country = await _unitOfWork.GetRepository<Country>().GetFirstOrDefaultAsync(predicate: x => x.CountryCode.ToLower() == hrData.CountryCode.ToLower()) ?? null;
                    var state = await _unitOfWork.GetRepository<State>().GetFirstOrDefaultAsync(predicate: x => x.StateCode.ToLower() == hrData.StateCode.ToLower()) ?? null;
                    var lga = await _unitOfWork.GetRepository<LGA>().GetFirstOrDefaultAsync(predicate: x => x.LGACode.ToLower() == hrData.LGACode.ToLower()) ?? null;
                    var location = await _unitOfWork.GetRepository<Location>().GetFirstOrDefaultAsync(predicate: x => x.LocationCode.ToLower() == hrData.LocationCode.ToLower()) ?? null;
                    var courtesy = await _unitOfWork.GetRepository<Courtesy>().GetFirstOrDefaultAsync(predicate: x => x.CourtesyCode.ToLower() == hrData.CourtesyCode.ToLower()) ?? null;

                    if (department != null) { model.DepartmentId = department.Id; }
                    if (division != null) { model.DivisionId = division.Id; }
                    if (unit != null) { model.UnitId = unit.Id; }
                    if (gradeLevel != null) { model.GradeLevelId = gradeLevel.Id; } 
                    model.DOB = DateTime.ParseExact(hrData.date_birth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.EmploymentDate = DateTime.ParseExact(hrData.date_Emp, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.DateConf = DateTime.ParseExact(hrData.date_conf, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.EffectiveDate = DateTime.ParseExact(hrData.effectiveDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.PreAppDate = DateTime.ParseExact(hrData.preAppDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.ProRetireDate = DateTime.ParseExact(hrData.projRetireDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.StaffType = hrData.TypeCode;
                    if (section != null) { model.SectionId = section.Id; }
                    if (title != null) { model.EmployeeTitleId = title.Id; }
                    if (maritalStatus != null) { model.MaritalStatusId = maritalStatus.Id; }
                    if (availability != null) { model.AvalaibilityStatusId = availability.Id; }
                    if (country != null) { model.CountryId = country.Id; }
                    if (state != null) { model.StateId = state.Id; }
                    if (lga != null) { model.LGAId = lga.Id; }
                    if (location != null) { model.LocationId = location.Id; }
                    if (courtesy != null) { model.CourtesyId = courtesy.Id; }
                }
                model.Status = Status.Active;
                model.AccessType = AccessType.Employee;
                _unitOfWork.GetRepository<Employee>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                //register the user in identity table

                return new EmployeeResponseModel() { Status = true, Message = ResponseMessage.CreatedSuccessful, Data = _mapper.Map<EmployeeModel>(model) };
            }
            return new EmployeeResponseModel() { Status = false, Message = ResponseMessage.RecordExist };
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

        public async Task<BaseResponse> RequestTransfer(Guid id, Guid divisionId, Guid departmentId, Guid sectionId, Guid? unitId)
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
                        SectionId = sectionId,
                        UnitId = unitId,
                        Emp_No = model.Emp_No,
                        ApprovalStatus = ApprovalStatus.Pending,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now
                    };
                    _unitOfWork.GetRepository<AppliedTransfer>().Insert(appliedTransfer);
                    await _unitOfWork.SaveChangesAsync();

                    //submit for approval
                    var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("transfer"));
                    var approvalProcessor = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: x => x.EmployeeId == model.Id && x.ApprovalLevel == Level.FirstLevel && x.ApprovalWorkItemId == approvalWorkItem.Id);

                    await _approvalBoardService.Create(new ApprovalBoard()
                    {
                        EmployeeId = model.Id,
                        ApprovalLevel = Level.FirstLevel,
                        Emp_No = model.Emp_No,
                        ApprovalWorkItemId = approvalWorkItem.Id,
                        ApprovalProcessorId = approvalProcessor.ProcessorIId.Value,
                        ApprovalProcessor = approvalProcessor.Processor,
                        ServiceId = model.Id,
                        Status = ApprovalStatus.Pending,
                        CreatedDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        CreatedBy = model.Emp_No
                    });

                    await _approvalBoardActiveLevelService.CreateOrUpdate(approvalWorkItem.Id, appliedTransfer.Id, Level.FirstLevel);

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
            var data = await GetAll(x => x.Status == Status.Active, "Department,Division,Unit,GradeLevel,Section,Location,MaritalStatus,Courtesy,Country,State,LGA,AvalaibilityStatus,EmployeeTitle");
            return data;
        }

        public async Task<Employee> GetByEmployerIdOrEmail(string employeeIdOrEmail)
        {
            var model = await _unitOfWork.GetRepository<Employee>().GetFirstOrDefaultAsync(predicate: c => c.Emp_No == employeeIdOrEmail || c.EmailAddress == employeeIdOrEmail, null, include: c => c.Include(i => i.Department).Include(i => i.Unit).Include(i => i.GradeLevel).Include(i => i.Division).Include(i => i.Section).Include(i => i.Location).Include(i => i.MaritalStatus).Include(i => i.Courtesy).Include(i => i.Country).Include(i => i.State).Include(i => i.LGA).Include(i => i.AvalaibilityStatus).Include(i => i.EmployeeTitle));
            return model;
        }

        public async Task<IEnumerable<Employee>> GetByDepartment(Guid departmentId)
        {
            var data = await GetAll(x => x.DepartmentId != departmentId, "Employee,Department,Division,Unit,GradeLevel,Section,Location,MaritalStatus,Courtesy,Country,State,LGA,AvalaibilityStatus,EmployeeTitle");
            return data;
        }

        public async Task<Employee> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<Employee>().GetFirstOrDefaultAsync(predicate: c => c.Id == id, include: c => c.Include(i => i.Department).Include(i => i.Unit).Include(i => i.GradeLevel).Include(i => i.Division).Include(i => i.Section).Include(i => i.Location).Include(i => i.MaritalStatus).Include(i => i.Courtesy).Include(i => i.Country).Include(i => i.State).Include(i => i.LGA).Include(i => i.AvalaibilityStatus).Include(i => i.EmployeeTitle));
            return model;
        }

        public async Task<IEnumerable<HREmpMst>> GetUnregisteredBaseEmployee()
        {
            var sql = "select * from HREmpMst";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HREmpMst> userList = new List<HREmpMst>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HREmpMst requester = new HREmpMst()
                    {
                        first_Name = reader["first_Name"].ToString(),
                        last_Name = reader["last_Name"].ToString(),
                        mid_Name = reader["mid_Name"].ToString(),
                        Employee_Email = reader["Employee_Email"].ToString(),
                        Emp_No = reader["Emp_No"].ToString(),
                    };
                    userList.Add(requester);
                }
                _sqlConnection.Close();
            }

            var registeredEmployee = await GetAll();

            List<HREmpMst> unRegisteredUser = new List<HREmpMst>();

            foreach (var item in userList)
            {
                if (registeredEmployee.Where(x => x.Emp_No.ToLower() == item.Emp_No.ToLower()).Count() == 0)
                {
                    unRegisteredUser.Add(item);
                }
            }
            return unRegisteredUser;
        }

        public async Task<IEnumerable<Employee>> GetAll(Expression<Func<Employee, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Employee>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<IEnumerable<Employee>> GetAllLowGradeEmployee(Guid employeeId)
        {
            try
            {
                var allGradeLevels = await _gradeLevelService.GetAll();
                var employee = await GetById(employeeId);
                var allEmployee = await GetAll();

                int employeeLevel = 0;

                List<Employee> lowGradeEmployee = new List<Employee>();

                foreach (var item in allGradeLevels)
                {
                    if (employee.GradeLevelId.ToString().ToLower() == item.Id.ToString().ToLower())
                    {
                        string formattedGrade = item.GradeCode.Replace("GL", string.Empty);
                        if (formattedGrade.ToLower() == "dr")
                            employeeLevel = 17;
                        if (formattedGrade.StartsWith("EX"))
                            employeeLevel = 16;
                        if (formattedGrade.ToLower() == "sp")
                            employeeLevel = 15;
                        else
                        {
                            employeeLevel = int.Parse(formattedGrade);
                        }

                        foreach (var employeeUnit in allEmployee)
                        {
                            if (employeeUnit.GradeLevelId != item.Id)
                            {
                                string formattedEmployeeGrade =  allGradeLevels.Where(x => x.Id == employeeUnit.GradeLevelId).FirstOrDefault().GradeCode.Replace("GL", string.Empty);
                                if (formattedEmployeeGrade.StartsWith("EX") && employeeLevel == 17)
                                    lowGradeEmployee.Add(employeeUnit);
                                else if (formattedEmployeeGrade.ToLower() == "sp" && (employeeLevel == 16 || employeeLevel == 17))
                                    lowGradeEmployee.Add(employeeUnit);
                                else if (formattedEmployeeGrade.All(char.IsDigit))
                                {
                                    var employeeUnitLevel = int.Parse(formattedEmployeeGrade);
                                    if (employeeLevel > employeeUnitLevel)
                                        lowGradeEmployee.Add(employeeUnit);
                                }
                            }
                        }
                    }
                }
                return lowGradeEmployee;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<BaseResponse> UpdateAccessType(Guid employeeId, AccessType accessType)
        {
            var employee = await GetById(employeeId);
            employee.AccessType = accessType;

            _unitOfWork.GetRepository<Employee>().Update(employee);
            await _unitOfWork.SaveChangesAsync();

            return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
        }
    }
}
