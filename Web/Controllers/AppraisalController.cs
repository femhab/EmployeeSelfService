using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Model;
using ViewModel.ResponseModel;
using Web.Helper.JWT;

namespace Web.Controllers
{
    public class AppraisalController : BaseController
    {
        private readonly IAppraisalRatingService _appraisalRatingService;
        private readonly IAppraisalCategoryService _appraisalCategoryService;
        private readonly IAppraisalCategoryItemService _appraisalCategoryItemService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IEmployeeAppraisalService _employeeAppraisalService;
        private readonly IAppraisalItemService _appraisalItemService;
        private readonly IAppraisalPeriodService _appraisalPeriodService;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IEmployeeService _employeeService;
        private readonly IContractService _contractService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public AppraisalController(IAppraisalRatingService appraisalRatingService, IAppraisalCategoryService appraisalCategoryService, IAppraisalCategoryItemService appraisalCategoryItemService, IEmployeeService employeeService, IMapper mapper, IEmployeeApprovalConfigService employeeApprovalConfigService, IAppraisalPeriodService appraisalPeriodService, IEmployeeAppraisalService employeeAppraisalService, IAppraisalItemService appraisalItemService, IApprovalBoardService approvalBoardService, IContractService contractService, INotificationService notificationService)
        {
            _appraisalRatingService = appraisalRatingService;
            _appraisalCategoryService = appraisalCategoryService;
            _appraisalCategoryItemService = appraisalCategoryItemService;
            _appraisalPeriodService = appraisalPeriodService;
            _appraisalItemService = appraisalItemService;
            _employeeService = employeeService;
            _approvalBoardService = approvalBoardService;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _employeeAppraisalService = employeeAppraisalService;
            _contractService = contractService;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                AppraisalViewModel appraisalViewModel = new AppraisalViewModel();
                var ratingList = await _appraisalRatingService.GetAll();
                var employee = await _employeeService.GetByEmployerIdOrEmail(authData.Emp_No);
                var categoryList = await _appraisalCategoryService.GetAll();
                var categoryItemList = (await _appraisalCategoryItemService.GetAll()).Where(x => x.StaffType.ToLower() == employee.StaffType.ToLower());
                var employeeList = await _employeeService.GetAll();
                var employeeAppraisal =await _employeeAppraisalService.GetByEmployee(authData.Id);
                var extractedCategory = new List<AppraisalCategory>();
                foreach (var item in categoryItemList)
                {
                    if(extractedCategory.Count() > 0)
                    {
                        if (extractedCategory.Where(x => x.AppraisalCategoryCode.ToLower() == item.AppraisalCategoryCode.ToLower()).Count() < 1)
                        {
                            extractedCategory.Add(item.AppraisalCategory);
                        }
                    }
                    else
                    {
                        extractedCategory.Add(item.AppraisalCategory);
                    }
                    
                }

                appraisalViewModel = _mapper.Map<AppraisalViewModel>(authData);
                appraisalViewModel.AppraisalRatings = _mapper.Map<IEnumerable<AppraisalRatingModel>>(ratingList);
                appraisalViewModel.AppraisalCategories = _mapper.Map<IEnumerable<AppraisalCategoryModel>>(extractedCategory);
                appraisalViewModel.AppraisalCategoryItems = _mapper.Map<IEnumerable<AppraisalCategoryItemModel>>(categoryItemList);
                appraisalViewModel.EmployeeList = _mapper.Map<IEnumerable<EmployeeModel>>(employeeList);
                appraisalViewModel.Employee = _mapper.Map<EmployeeModel>(employee);
                appraisalViewModel.EmployeeAppraisal = _mapper.Map<IEnumerable<EmployeeAppraisalModel>>(employeeAppraisal);

                return View(appraisalViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        public async Task<ActionResult> PerformanceReview(Guid appraisalId)
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                AppraisalViewModel appraisalViewModel = new AppraisalViewModel();
                var employee = await _employeeService.GetByEmployerIdOrEmail(authData.Emp_No);
                var categoryList = await _appraisalCategoryService.GetAll();
                var ratingList = await _appraisalRatingService.GetAll();

                var response = new List<AppraisalItem>();
                var empAppraisal = new EmployeeAppraisal();
                var board = new ApprovalBoard();
                var unsignedBoard = new ApprovalBoard();
                var signedBoard = new ApprovalBoard();

                response = (await _appraisalItemService.GetByAppraisal(appraisalId)).ToList();
                unsignedBoard = await _approvalBoardService.GetUnsignedAppraisal(appraisalId);
                signedBoard = await _approvalBoardService.GetreviewedAppraisal(appraisalId);
                empAppraisal = await _employeeAppraisalService.GetById(appraisalId);

                var contractReview = (await _contractService.GetByEmployee(empAppraisal.EmployeeId)).Where(x => x.CreatedDate.Year == DateTime.Now.AddYears(-1).Year && unsignedBoard.Emp_No.ToLower() == empAppraisal.Emp_No.ToLower()).FirstOrDefault();
                var contractItem = (contractReview != null) ? await _contractService.GetItemByObjectiveId(contractReview.Id) : null;

                var extractedCategory = new List<AppraisalCategory>();
                foreach (var item in response)
                {
                    if (extractedCategory.Count() > 0)
                    {
                        if (extractedCategory.Where(x => x.AppraisalCategoryCode.ToLower() == item.AppraisalCategory.AppraisalCategoryCode.ToLower()).Count() < 1)
                        {
                            extractedCategory.Add(item.AppraisalCategory);
                        }
                    }
                    else
                    {
                        extractedCategory.Add(item.AppraisalCategory);
                    }

                }

                appraisalViewModel = _mapper.Map<AppraisalViewModel>(authData);

                if (empAppraisal.Employee.GradeLevel.GradeCode == "GL08" || empAppraisal.Employee.GradeLevel.GradeCode == "GL09" || empAppraisal.Employee.GradeLevel.GradeCode == "GL10" || empAppraisal.Employee.GradeLevel.GradeCode == "GL11" || empAppraisal.Employee.GradeLevel.GradeCode == "GL12" || empAppraisal.Employee.GradeLevel.GradeCode == "GL13" || empAppraisal.Employee.GradeLevel.GradeCode == "GL14" || empAppraisal.Employee.GradeLevel.GradeCode == "GLDR")
                {
                    appraisalViewModel.IsContractible = true;
                }

                appraisalViewModel.AppraisalRatings = _mapper.Map<IEnumerable<AppraisalRatingModel>>(ratingList);
                appraisalViewModel.AppraisalCategories = _mapper.Map<IEnumerable<AppraisalCategoryModel>>(extractedCategory);
                appraisalViewModel.Employee = _mapper.Map<EmployeeModel>(employee);
                appraisalViewModel.TargetAppraisal = _mapper.Map<EmployeeAppraisalModel>(empAppraisal);
                appraisalViewModel.AppraisalItem = _mapper.Map<IEnumerable<AppraisalItemModel>>(response);
                appraisalViewModel.ContractItem = _mapper.Map<IEnumerable<ContractItemModel>>(contractItem);
                appraisalViewModel.ContractObjective = _mapper.Map<ContractObjectiveModel>(contractReview);

                return View("PerformanceReview", appraisalViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [Route("Appraisal-Access")]
        public async Task<ActionResult> AccessAppraisal(Guid boardId)
        {
            var authData = JwtHelper.GetAuthData(Request);
            if (authData == null)
            {
                return RedirectToAction("Signout", "Employee");
            }

            AppraisalViewModel appraisalViewModel = new AppraisalViewModel();
            var employee = await _employeeService.GetByEmployerIdOrEmail(authData.Emp_No);
            var categoryList = await _appraisalCategoryService.GetAll();
            var ratingList = await _appraisalRatingService.GetAll();

            var response = new List<AppraisalItem>();
            var empAppraisal = new EmployeeAppraisal();
            var board = new ApprovalBoard();
            var unsignedBoard = new ApprovalBoard();
            var signedBoard = new ApprovalBoard();

            board = await _approvalBoardService.GetById(boardId); //get board detail
            response = (await _appraisalItemService.GetByAppraisal(board.ServiceId)).ToList();
            empAppraisal = await _employeeAppraisalService.GetById(board.ServiceId);
            var contractReview = (await _contractService.GetByEmployee(empAppraisal.EmployeeId)).Where(x => x.CreatedDate.Year == DateTime.Now.AddYears(-1).Year && board.Emp_No.ToLower() == empAppraisal.Emp_No.ToLower()).FirstOrDefault();
            var contractItem = (contractReview != null) ? await _contractService.GetItemByObjectiveId(contractReview.Id) : null;

            var extractedCategory = new List<AppraisalCategory>();
            foreach (var item in response)
            {
                if (extractedCategory.Count() > 0)
                {
                    if (extractedCategory.Where(x => x.AppraisalCategoryCode.ToLower() == item.AppraisalCategory.AppraisalCategoryCode.ToLower()).Count() < 1)
                    {
                        extractedCategory.Add(item.AppraisalCategory);
                    }
                }
                else
                {
                    extractedCategory.Add(item.AppraisalCategory);
                }

            }

            appraisalViewModel = _mapper.Map<AppraisalViewModel>(authData);

            if (empAppraisal.Employee.GradeLevel.GradeCode == "GL08" || empAppraisal.Employee.GradeLevel.GradeCode == "GL09" || empAppraisal.Employee.GradeLevel.GradeCode == "GL10" || empAppraisal.Employee.GradeLevel.GradeCode == "GL11" || empAppraisal.Employee.GradeLevel.GradeCode == "GL12" || empAppraisal.Employee.GradeLevel.GradeCode == "GL13" || empAppraisal.Employee.GradeLevel.GradeCode == "GL14" || empAppraisal.Employee.GradeLevel.GradeCode == "GLDR")
            {
                appraisalViewModel.IsContractible = true;
            }

            appraisalViewModel.AppraisalRatings = _mapper.Map<IEnumerable<AppraisalRatingModel>>(ratingList);
            appraisalViewModel.AppraisalCategories = _mapper.Map<IEnumerable<AppraisalCategoryModel>>(extractedCategory);
            appraisalViewModel.Employee = _mapper.Map<EmployeeModel>(employee);
            appraisalViewModel.TargetAppraisal = _mapper.Map<EmployeeAppraisalModel>(empAppraisal);
            appraisalViewModel.TargetBoard = _mapper.Map<ApprovalBaordModel>(board);
            appraisalViewModel.AppraisalItem = _mapper.Map<IEnumerable<AppraisalItemModel>>(response);
            appraisalViewModel.ContractItem = _mapper.Map<IEnumerable<ContractItemModel>>(contractItem);
            appraisalViewModel.ContractObjective = _mapper.Map<ContractObjectiveModel>(contractReview);

            return View(appraisalViewModel);
        }


        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateAppraisal(Guid id, List<string> catItemWeight)
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

                    //get first level approver
                    var approverConfig = await _employeeApprovalConfigService.GetByServiceLevel(authData.Id, "appraisal", Level.FirstLevel);
                    var appraisalPeriod = await _appraisalPeriodService.GetActivePeriod();
                    var ratingList = await _appraisalRatingService.GetAll();
                    var categoryItemList = await _appraisalCategoryItemService.GetAll();
                    if (appraisalPeriod == null)
                    {

                    }

                    if (approverConfig != null)
                    {
                        var employeeAppraisal = new EmployeeAppraisal()
                        {
                            EmployeeId = authData.Id,
                            Emp_No = authData.Emp_No,
                            LastRatingManagerId = authData.Emp_No,
                            LastRatingManagerName = authData.LastName + authData.LastName,
                            NextRatingManagerId = approverConfig.Emp_No,
                            NextRatingManagerName = approverConfig.Employee.LastName + " " + approverConfig.Employee.FirstName,
                            AppraisalPeriodId = appraisalPeriod.Id,
                            Id = Guid.NewGuid(),
                            CreatedDate = DateTime.Now,
                            TotalScore = 0
                        };

                        var appraisalItemList = new List<AppraisalItem>();

                        foreach (var item in catItemWeight)
                        {
                            Guid category = Guid.Parse(item.Split("/")[0]);
                            Guid categoryItem = Guid.Parse(item.Split("/")[1]);
                            Guid rating = Guid.Parse(item.Split("/")[2]);

                            appraisalItemList.Add(new AppraisalItem() { AppraisalCategoryId = category, AppraisalCategoryItemId = categoryItem, AppraisalRatingId = rating, EmployeeAppraisalId = employeeAppraisal.Id, Id = Guid.NewGuid(), CreatedDate = DateTime.Now });

                            var ratingScore = 0;
                            var itemScore = 0;
                            foreach (var ratingItem in ratingList)
                            {
                                if(rating == ratingItem.Id)
                                {
                                    ratingScore = ratingItem.Weight;
                                }
                            }
                            foreach (var catItem in categoryItemList)
                            {
                                if (categoryItem == catItem.Id)
                                {
                                    itemScore = catItem.Weight;
                                }
                            }
                            employeeAppraisal.TotalScore = employeeAppraisal.TotalScore + (itemScore * ratingScore);
                        }

                        var appraisalCreate = await _employeeAppraisalService.Create(employeeAppraisal);
                        if (appraisalCreate.Status)
                        {
                            await _appraisalItemService.Create(appraisalItemList);
                        }

                        return Json(new
                        {
                            status = appraisalCreate.Status,
                            message = appraisalCreate.Message
                        });
                    }
                    

                    return Json(new
                    {
                        status = false,
                        message = "You don't have approval configuration for appraisal yet, reach out to the admin."
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
        public async Task<ActionResult> AddAppraisal(List<string> catItemWeight)
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

                    var ratingList = await _appraisalRatingService.GetAll();
                    var categoryItemList = await _appraisalCategoryItemService.GetAll();

                    //get first level approver
                    var approverConfig = await _employeeApprovalConfigService.GetByServiceLevel(authData.Id, "appraisal", Level.FirstLevel);
                    var appraisalPeriod = (await _appraisalPeriodService.GetActivePeriod());
                    if(appraisalPeriod == null || appraisalPeriod.StartDate > DateTime.Now)
                    {
                        return Json(new
                        {
                            status = false,
                            message = "No active appraisal at this period."
                        });
                    }
                    if (approverConfig != null)
                    {
                        var employeeAppraisal = new EmployeeAppraisal()
                        {
                            EmployeeId = authData.Id,
                            Emp_No = authData.Emp_No,
                            LastRatingManagerId = authData.Emp_No,
                            LastRatingManagerName = authData.LastName + authData.LastName,
                            NextRatingManagerId = approverConfig.Emp_No,
                            NextRatingManagerName = approverConfig.Employee.LastName + approverConfig.Employee.FirstName,
                            AppraisalPeriodId = appraisalPeriod.Id,
                            Id = Guid.NewGuid(),
                            CreatedDate = DateTime.Now,
                            TotalScore = 0
                        };

                        var appraisalItemList = new List<AppraisalItem>();

                        foreach (var item in catItemWeight)
                        {
                            Guid category = Guid.Parse(item.Split("/")[0]);
                            Guid categoryItem = Guid.Parse(item.Split("/")[1]);
                            Guid rating = Guid.Parse(item.Split("/")[2]);

                            appraisalItemList.Add(new AppraisalItem() { AppraisalCategoryId = category, AppraisalCategoryItemId = categoryItem, AppraisalRatingId = rating, EmployeeAppraisalId = employeeAppraisal.Id, Id = Guid.NewGuid(), CreatedDate = DateTime.Now });

                            var ratingScore = 0;
                            var ratingHighScore = 0;
                            var itemScore = 0;
                            foreach (var ratingItem in ratingList)
                            {
                                if (rating == ratingItem.Id)
                                {
                                    ratingScore = ratingItem.Weight;
                                    
                                }
                                if (ratingItem.Weight > ratingHighScore)
                                    ratingHighScore = ratingItem.Weight;
                            }
                            var selectedItem = categoryItemList.Where(x => x.Id == categoryItem).FirstOrDefault();
                            itemScore = selectedItem.Weight;

                            employeeAppraisal.TotalScore = employeeAppraisal.TotalScore + (itemScore * ratingScore);
                            employeeAppraisal.TotalNetScore = employeeAppraisal.TotalNetScore + (itemScore * ratingHighScore);
                        }

                        var appraisalCreate = await _employeeAppraisalService.Create(employeeAppraisal);
                        if (appraisalCreate.Status)
                        {
                            await _appraisalItemService.Create(appraisalItemList);
                        }

                        return Json(new
                        {
                            status = appraisalCreate.Status,
                            message = appraisalCreate.Message
                        });
                    }
                    return Json(new
                    {
                        status = false,
                        message = "You don't have approval configuration for appraisal yet, reach out to the admin."
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
        public async Task<ActionResult> getAppraisalById(Guid appraisalId, bool isAppraisalView = false)
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
                    var response = new List<AppraisalItem>();
                    var empAppraisal = new EmployeeAppraisal();
                    var board = new ApprovalBoard();
                    var unsignedBoard = new ApprovalBoard();
                    var signedBoard = new ApprovalBoard();
                    var categoryList = await _appraisalCategoryService.GetAll();

                    if (isAppraisalView == false)
                    {
                        board = await _approvalBoardService.GetById(appraisalId); //get board detail
                        response = (await _appraisalItemService.GetByAppraisal(board.ServiceId)).ToList();
                        empAppraisal = await _employeeAppraisalService.GetById(board.ServiceId);
                    }
                    else
                    {
                        response = (await _appraisalItemService.GetByAppraisal(appraisalId)).ToList();
                        unsignedBoard = await _approvalBoardService.GetUnsignedAppraisal(appraisalId);
                        signedBoard = await _approvalBoardService.GetreviewedAppraisal(appraisalId);
                        empAppraisal = await _employeeAppraisalService.GetById(appraisalId);
                    }
                    var contractReview = (await _contractService.GetByEmployee(empAppraisal.EmployeeId)).Where(x => x.CreatedDate.Year == DateTime.Now.AddYears(-1).Year && board.Emp_No.ToLower() == empAppraisal.Emp_No.ToLower()).FirstOrDefault();
                    var contractItem = (contractReview != null) ? await _contractService.GetItemByObjectiveId(contractReview.Id): null;

                    var extractedCategory = new List<AppraisalCategory>();
                    foreach (var item in response)
                    {
                        if (extractedCategory.Count() > 0)
                        {
                            if (extractedCategory.Where(x => x.AppraisalCategoryCode.ToLower() == item.AppraisalCategory.AppraisalCategoryCode.ToLower()).Count() < 1)
                            {
                                extractedCategory.Add(item.AppraisalCategory);
                            }
                        }
                        else
                        {
                            extractedCategory.Add(item.AppraisalCategory);
                        }

                    }

                    return Json(new
                    {
                        status = (response != null) ? true : false,
                        data = new { detail = response, level = board.ApprovalLevel, serviceId = board.ServiceId, unsigned = (unsignedBoard != null) ? true : false, apparaisalid = appraisalId, strenght = empAppraisal.Strenght, weekness = empAppraisal.Weekness, development = empAppraisal.Development, counselling = empAppraisal.Counselling, redeployment = empAppraisal.Redeployment, action = empAppraisal.DisciplinaryAction, training = empAppraisal.Training, promotion = empAppraisal.Promotion, appraisalTarget = empAppraisal.AppraisalTarget, others = empAppraisal.OtherDetail, categories = extractedCategory, appraisalScore = empAppraisal, contract = (contractReview != null) ? contractReview: null , objectiveItem = contractItem, employeeReview = board.EmployeeReview, managerSignOff = (signedBoard != null)? signedBoard.ManagerSignOff:  false, appraiseeComment = empAppraisal.AppraiseeComment, areaOfImprovement  = empAppraisal.AreaOfImprovement}
                    }) ;

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
        public async Task<ActionResult> SignOffAppraisal(Guid appraisalId, string appraiseeComment, string appraiseeImprove)
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

                    var appraisal = await _employeeAppraisalService.GetById(appraisalId);
                    appraisal.AppraiseeComment = (appraiseeComment == null) ? appraisal.AppraiseeComment : appraiseeComment;
                    appraisal.AreaOfImprovement = (appraiseeImprove == null)? appraisal.AreaOfImprovement: appraiseeImprove;
                    appraisal.IsEmployeeSignOff = true;
                    var response = await _employeeAppraisalService.Update(appraisal);

                    await _notificationService.CreateNotification(NotificationAction.SignOffTitle, NotificationAction.EmployeeSignOffMessage, authData.Id, false, false) ;
                    //await _notificationService.CreateNotification(NotificationAction.SignOffTitle, NotificationAction.AdminSignOffRecieveMessage, authData.Id, false, false);

                    return Json(new
                    {
                        status = response.Status,
                        data = response.Message
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