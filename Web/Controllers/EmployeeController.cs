using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Helper;
using ViewModel.Model;
using ViewModel.ResponseModel;
using Web.Helper.JWT;

namespace Web.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IApprovalWorkItemService _approvalWorkItemService;
        private readonly IAuthService _authService;
        private readonly IDivisionService _divisionService;
        private readonly IDepartmentService _departmentService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IEmployeeNOKDetailService _employeeNOKDetailService;
        private readonly IEmployeeFamilyDependentService _employeeFamilyDependentService;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IEmployeeAddressService _employeeAddressService;
        private readonly IEmployeeEducationalDetailService _employeeEducationalDetailService;
        private readonly IEducationalGradeService _educationalGradeService;
        private readonly IEducationalLevelService _educationalLevelService;
        private readonly IEducationalQualificationService _educationalQualificationService;
        private readonly IRoleService _roleService;
        private readonly IUserRoleService _userRoleService;
        private readonly IRelationshipService _relationshipService;
        private readonly ISectionService _sectionService;
        private readonly IDocumentService _documentService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, IRoleService roleService, IDepartmentService departmentService, IAuthService authService, IApprovalWorkItemService approvalWorkItemService, IEmployeeNOKDetailService employeeNOKDetailService, IEmployeeAddressService employeeAddressService, IRelationshipService relationshipService, IEmployeeFamilyDependentService employeeFamilyDependentService, IEmployeeApprovalConfigService employeeApprovalConfigService, IUserRoleService userRoleService, IEmployeeEducationalDetailService employeeEducationalDetailService, IEducationalGradeService educationalGradeService, IEducationalLevelService educationalLevelService, IEducationalQualificationService educationalQualificationService, IDivisionService divisionService,ISectionService sectionService, IApprovalBoardService approvalBoardService, IDocumentService documentService, IHostingEnvironment hostingEnvironment)
        {
            _employeeService = employeeService;
            _approvalWorkItemService = approvalWorkItemService;
            _authService = authService;
            _departmentService = departmentService;
            _divisionService = divisionService;
            _employeeNOKDetailService = employeeNOKDetailService;
            _employeeFamilyDependentService = employeeFamilyDependentService;
            _employeeAddressService = employeeAddressService;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _employeeEducationalDetailService = employeeEducationalDetailService;
            _educationalGradeService = educationalGradeService;
            _educationalLevelService = educationalLevelService;
            _educationalQualificationService = educationalQualificationService;
            _roleService = roleService;
            _hostingEnvironment = hostingEnvironment;
            _userRoleService = userRoleService;
            _sectionService = sectionService;
            _relationshipService = relationshipService;
            _approvalBoardService = approvalBoardService;
            _documentService = documentService;
            _mapper = mapper;
        }

        //route section
        public IActionResult Index()
        {
            return View();
        }

        [Route("Employee")]
        public async Task<ActionResult> Enroll()
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                EmployeeViewModel employeeViewModel = new EmployeeViewModel();
                var deptList = await _departmentService.GetAll();
                var roleList = await _roleService.GetAll();
                var employeeList = await _employeeService.GetAll();
                var employeeListResponse = await _employeeService.GetUnregisteredBaseEmployee();

                employeeViewModel = _mapper.Map<EmployeeViewModel>(authData);
                employeeViewModel.Employeee = _mapper.Map<IEnumerable<HRUserModel>>(employeeListResponse);
                employeeViewModel.Roles = _mapper.Map<IEnumerable<RoleModel>>(roleList);
                employeeViewModel.Departments = _mapper.Map<IEnumerable<DepartmentModel>>(deptList);
                employeeViewModel.EmployeeList = _mapper.Map<IEnumerable<EmployeeModel>>(employeeList);

                return View(employeeViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        //[Route("Profile")]
        public async Task<ActionResult> Profile(string id)
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }
                if(id.ToLower() == "default")
                {
                    id = authData.Id.ToString();
                    authData.AccessType = ViewModel.Enumeration.AccessTypeEnum.Employee;
                }
                else
                {
                    authData.Id = Guid.Parse(id);
                }

                EmployeeProfileViewModel profileViewModel = new EmployeeProfileViewModel();
                var employee = await _employeeService.GetById(Guid.Parse(id));
                var approvalWorkItem = await _approvalWorkItemService.GetAll();
                var relationshipList = await _relationshipService.GetAll();
                var nokList = await _employeeNOKDetailService.GetByEmployee(authData.Id);
                foreach (var item in nokList)
                {
                    var document = await _documentService.GetByReference(item.Id, DocumentType.NextOfKin);
                    item.PictureUrl = (document != null) ?  $"{Request.Scheme}://{Request.Host}" +
                        $"{document.DocumentUrl}": null;
                }
                var dependentList = await _employeeFamilyDependentService.GetByEmployee(authData.Id);
                var userRoles = await _userRoleService.GetAll();
                var workItems = await _approvalWorkItemService.GetAll();
                var eduLevel = await _educationalLevelService.GetAll();
                var eduGrade = await _educationalGradeService.GetAll();
                var eduQual = await _educationalQualificationService.GetAll();
                var eduDetail = await _employeeEducationalDetailService.GetByEmployee(authData.Id);
                var approvalList = await _employeeApprovalConfigService.GetByEmployee(authData.Id);
                var division = await _divisionService.GetAll();
                var department = await _departmentService.GetAll();
                var section = await _sectionService.GetAll();
                //var unit = await _employeeApprovalConfigService.GetByEmployee(authData.Id);

                profileViewModel = _mapper.Map<EmployeeProfileViewModel>(authData);
                profileViewModel.Employee = _mapper.Map<EmployeeModel>(employee);
                profileViewModel.ApprovalWorkItem = _mapper.Map<IEnumerable<ApprovalWorkItemModel>>(approvalWorkItem);
                profileViewModel.Relationshiop = _mapper.Map<IEnumerable<RelationshipModel>>(relationshipList);
                profileViewModel.NOKDetails = _mapper.Map<IEnumerable<EmployeeNOKDetailModel>>(nokList);
                profileViewModel.Dependents = _mapper.Map<IEnumerable<EmployeeFamilyDependentModel>>(dependentList);
                profileViewModel.UserRoles = _mapper.Map<IEnumerable<UserRoleModel>>(userRoles);
                profileViewModel.ApprovalWorkItem = _mapper.Map<IEnumerable<ApprovalWorkItemModel>>(workItems);
                profileViewModel.EducationalGrade = _mapper.Map<IEnumerable<EducationalGradeModel>>(eduGrade);
                profileViewModel.EducationalLevel = _mapper.Map<IEnumerable<EducationalLevelModel>>(eduLevel);
                profileViewModel.EducationalQualification = _mapper.Map<IEnumerable<EducationalQualificationModel>>(eduQual);
                profileViewModel.EmployeeEducationDetail = _mapper.Map<IEnumerable<EmployeeEducationDetailModel>>(eduDetail);
                profileViewModel.EmployeeApprovalconfig = _mapper.Map<IEnumerable<EmployeeApprovalconfigModel>>(approvalList);
                profileViewModel.Division = _mapper.Map<IEnumerable<DivisionModel>>(division);
                profileViewModel.Department = _mapper.Map<IEnumerable<DepartmentModel>>(department);
                profileViewModel.Section = _mapper.Map<IEnumerable<SectionModel>>(section);
                return View(profileViewModel);
            }
            catch(Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [Route("Transfer")]
        public IActionResult Transfer()
        {
            return View();
        }

        //action section
        [Route("Register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(string email, string password, string confirmPassword, string firstName, string lastName, string userName, string empNo, List<Guid> roleId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authData = JwtHelper.GetAuthData(Request);
                    if (authData == null)
                    {
                        return RedirectToAction("Signout", "Employee");
                    }

                    if (password.ToLower() == confirmPassword.ToLower())
                    {
                        var authRegister = await _authService.Register(email, password, lastName, firstName, userName, empNo, roleId); //, authData.Emp_No
                        if (authRegister.status)
                        {
                            //await _emailHelper.SendQuickMail(model.Email, "Registration Successful", $"Congratulations {model.FirstName} <br/>You are onbaorded successfully on Employee Self Service Platform. Your temporary password is {model.Password}, Please visit here ... to reset your password.<br/> If this is a wrong notification, please reach out on the hotline. <br/>Regards");
                            //return RedirectToAction("Index", "Dashboard");
                            return Json(new
                            {
                                authRegister.status,
                                authRegister.message
                            });
                        }
                        return Json(new
                        {
                            authRegister.status,
                            authRegister.message
                        });
                    }                      
                    return Json(new
                    {
                        status = false,
                        message = "Please check the password"
                    });
                }
                return Json(new
                {
                    status = false,
                    message = "Error with Current Request"
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        
        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddNextOfKin([FromForm] NokRequestModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authData = JwtHelper.GetAuthData(Request);
                    if (authData == null)
                    {
                        return RedirectToAction("Signout", "Employee");
                    }

                    var dob = DateTime.ParseExact(model.NokDOB, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var nokDetail = new EmployeeNOKDetail()
                    {
                        Emp_No = authData.Emp_No,
                        EmployeeId = authData.Id,
                        FirstName = model.NokFirstName,
                        LastName = model.NokLastName,
                        PhoneNumber = model.NokPhonenumber,
                        RelationshipId = model.nokRelationShipId,
                        Email = model.NokEmail,
                        DOB = dob,
                        Status = ApprovalStatus.Pending,
                        Address = model.NokAddress,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        IsEmergencyContact = model.IsEmergency
                    };

                    var path = $"wwwroot/Uploads/Documents/NextOfKin/{nokDetail.Id}/";
                    var upload = await FileUpload.UploadFile(model.NokPic, null, false, path);


                    if (!string.IsNullOrEmpty(upload))
                    {
                        var data = new Document()
                        {
                            RerenceId = nokDetail.Id,
                            Type = DocumentType.NextOfKin,
                            DocumentUrl = upload,
                            Id = Guid.NewGuid(),
                            CreatedDate = DateTime.Now
                        };
                        var status = await _documentService.CreateOrUpdate(data);
                    }

                    BaseResponse response = await _employeeNOKDetailService.Create(nokDetail);
                   
                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message
                    });
                }
                return Json(new
                {
                    status = false,
                    message = "Error with Current Request"
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddDependent(string depFirstName, string depLastName, string depPhonenumber, Guid depRelationShipId, string depAddress, string depDOB)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authData = JwtHelper.GetAuthData(Request);
                    if (authData == null)
                    {
                        return RedirectToAction("Signout", "Employee");
                    }

                    var dob = DateTime.ParseExact(depDOB, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var depDetail = new EmployeeFamilyDependent()
                    {
                        Emp_No = authData.Emp_No,
                        EmployeeId = authData.Id,
                        FirstName = depFirstName,
                        LastName = depLastName,
                        PhoneNumber = depPhonenumber,
                        RelationshipId = depRelationShipId,
                        DOB = dob,
                        Status = ApprovalStatus.Pending,
                        Address = depAddress,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now
                    };

                    var response = await _employeeFamilyDependentService.Create(depDetail);

                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message
                    });
                }
                return Json(new
                {
                    status = false,
                    message = "Error with Current Request"
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddApprovalConfig(Guid workItem, Guid employeeId, string empNo, Dictionary<int, Guid> levelApproval, int maxcount)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authData = JwtHelper.GetAuthData(Request);
                    if (authData == null)
                    {
                        return RedirectToAction("Signout", "Employee");
                    }

                    List<EmployeeApprovalConfig> approvalConfigList = new List<EmployeeApprovalConfig>();
                    foreach (var item in levelApproval)
                    {
                        if(item.Value == employeeId)
                        {
                            return Json(new
                            {
                                status = false,
                                message = "A user can not approve his/her own request"
                            });
                        }
                        if (approvalConfigList.Where(x => x.ProcessorIId == item.Value).Count() == 0)
                        {
                            var employee = await _employeeService.GetById(item.Value);
                            var approvalConfig = new EmployeeApprovalConfig()
                            {
                                Emp_No = empNo,
                                EmployeeId = employeeId,
                                ApprovalWorkItemId = workItem,
                                ProcessorIId = item.Value,
                                Processor = employee.LastName + "-" + employee.FirstName,
                                ApprovalLevel = (Level)item.Key,
                                Id = Guid.NewGuid(),
                                CreatedDate = DateTime.Now
                            };
                            approvalConfigList.Add(approvalConfig);
                        }
                        else if ((approvalConfigList.Where(x => x.ProcessorIId == item.Value).Count() > 0))
                        {
                            return Json(new
                            {
                                status = false,
                                message = "A Manager can only approve a level(e.g only first level etc)"
                            });
                        }
                    }

                    var approvalCount = new EmployeeApprovalCount()
                    {
                        ApprovalWorkItemId = workItem,
                        Emp_No = empNo,
                        EmployeeId = employeeId,
                        MaximumCount = maxcount,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now
                    };

                    var approvalCountResponse = await _employeeApprovalConfigService.SetApprovalCount(approvalCount);
                    var approvalConfigResponse = await _employeeApprovalConfigService.CreateUpdate(approvalConfigList);

                    return Json(new
                    {
                        status = (approvalConfigResponse.Status && approvalCountResponse.Status) ? true : false,
                        message = (approvalConfigResponse.Status && approvalCountResponse.Status) ? approvalConfigResponse.Message : "Oops! Failed to create configuration. Please try again"
                    });
                }
                return Json(new
                {
                    status = false,
                    message = "Error with Current Request"
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RequestTransfer(Guid newDivision, Guid newDepartment, Guid newSection, Guid? newUnit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authData = JwtHelper.GetAuthData(Request);
                    if (authData == null)
                    {
                        return RedirectToAction("Signout", "Employee");
                    }

                    var approverConfig = await _employeeApprovalConfigService.GetByServiceLevel(authData.Id, "transfer", Level.FirstLevel);

                    if(approverConfig != null)
                    {
                        var response = await _employeeService.RequestTransfer(authData.Id, newDivision, newDepartment, newSection, newUnit); //dependent service
                        return Json(new
                        {
                            status = response.Status,
                            message = response.Message
                        });
                    }

                    return Json(new
                    {
                        status = false,
                        message = "You don't have approval configuration for transfer yet, reach out to the admin."
                    });
                }
                return Json(new
                {
                    status = false,
                    message = "Error with Current Request"
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RequestNameChange(string lastName, string firstName, string email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authData = JwtHelper.GetAuthData(Request);
                    if (authData == null)
                    {
                        return RedirectToAction("Signout", "Employee");
                    }
                    var response = await _employeeService.RequestBasicInfoChange(authData.Id, lastName, firstName, email); //dependent service

                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message ?? "Failed to create Next of Kin Detail."
                    });
                }
                return Json(new
                {
                    status = false,
                    message = "Error with Current Request"
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteEmployee(Guid employeeId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authData = JwtHelper.GetAuthData(Request);
                    if (authData == null)
                    {
                        return RedirectToAction("Signout", "Employee");
                    }

                    await _authService.DeleteUser(employeeId);
                    var response = await _employeeService.Delete(employeeId); 

                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message ?? "Failed to create Next of Kin Detail."
                    });
                }
                return Json(new
                {
                    status = false,
                    message = "Error with Current Request"
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddEmployeeAddress(string state, string city, string streetAddress, string country, string stateOfOrigin, string lgOfOrigin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authData = JwtHelper.GetAuthData(Request);
                    if (authData == null)
                    {
                        return RedirectToAction("Signout", "Employee");
                    }

                    var addressDetail = new EmployeeAddress()
                    {
                        EmployeeId = authData.Id,
                        Emp_No = authData.Emp_No,
                        Country = country,
                        State = state,
                        City = city,
                        StreetAddress = streetAddress,
                        StateOfOrigin = stateOfOrigin,
                        LGOfOrigin = lgOfOrigin,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        Status = ApprovalStatus.Pending
                    };

                    var response = await _employeeAddressService.Create(addressDetail); //dependent service

                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message ?? "Failed to create Next of Kin Detail."
                    });
                }
                return Json(new
                {
                    status = false,
                    message = "Error with Current Request"
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddEducationaldetail(string institution, Guid level, Guid qualification, string course, Guid grade, string startDate, string endDate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authData = JwtHelper.GetAuthData(Request);
                    if (authData == null)
                    {
                        return RedirectToAction("Signout", "Employee");
                    }

                    var dateFrom = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var dateTo = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var educationDetail = new EmployeeEducationDetail()
                    {
                        Emp_No = authData.Emp_No,
                        EmployeeId = authData.Id,
                        Institution = institution,
                        Course = course,
                        EducationalLevelId = level,
                        EducationalGradeId = grade,
                        EducationalQualificationId = qualification,
                        StartDate = dateFrom,
                        EndDate = dateTo,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now
                    };

                    var response = await _employeeEducationalDetailService.Create(educationDetail);

                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message ?? "Failed to create Next of Kin Detail."
                    });
                }
                return Json(new
                {
                    status = false,
                    message = "Error with Current Request"
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        public async Task<IActionResult> Signout()
        {
            try
            {
                var result = await _authService.Logout();

                return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult> CountProcessor(int approvalCount)
        {
            try
            {
                return Json(new
                {
                    status = true,
                    message = "Processing",
                    data = approvalCount
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> GetTransferById(Guid transferId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authData = JwtHelper.GetAuthData(Request);
                    if (authData == null)
                    {
                        return RedirectToAction("Signout", "Employee");
                    }

                    var board = await _approvalBoardService.GetById(transferId);
                    var employee = await _employeeService.GetById(board.EmployeeId);
                    var response = await _employeeService.GetTransferById(board.ServiceId);
                    return Json(new
                    {
                        status = (response != null) ? true : false,
                        data = new { currentDivision = employee.Division.Descc, currentDepartment = employee.Department.Descc, currentSection = employee.Section.Descc, currentUnit = (employee.Unit != null) ? employee.Unit.Descc : "", newDivision = response.Division.Descc, newDepartment = response.Department.Descc, newSection = response.Section.Descc, newUnit = (response.Unit != null) ? response.Unit.Descc : "", serviceId = response.Id, level = board.ApprovalLevel, employeeId = employee.Emp_No, employeeName = employee.LastName + " " + employee.FirstName }
                    });

                }
                return Json(new
                {
                    status = false,
                    message = "Error with Current Request"
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }
    }
}