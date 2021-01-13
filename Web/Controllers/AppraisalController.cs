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
        private readonly IMapper _mapper;

        public AppraisalController(IAppraisalRatingService appraisalRatingService, IAppraisalCategoryService appraisalCategoryService, IAppraisalCategoryItemService appraisalCategoryItemService, IEmployeeService employeeService, IMapper mapper, IEmployeeApprovalConfigService employeeApprovalConfigService, IAppraisalPeriodService appraisalPeriodService, IEmployeeAppraisalService employeeAppraisalService, IAppraisalItemService appraisalItemService, IApprovalBoardService approvalBoardService)
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
                var categoryList = await _appraisalCategoryService.GetAll();
                var categoryItemList = await _appraisalCategoryItemService.GetAll();
                var employeeList = await _employeeService.GetAll();
                var employee = await _employeeService.GetByEmployerIdOrEmail(authData.Emp_No);
                var employeeAppraisal =await _employeeAppraisalService.GetByEmployee(authData.Id);

                appraisalViewModel = _mapper.Map<AppraisalViewModel>(authData);
                appraisalViewModel.AppraisalRatings = _mapper.Map<IEnumerable<AppraisalRatingModel>>(ratingList);
                appraisalViewModel.AppraisalCategories = _mapper.Map<IEnumerable<AppraisalCategoryModel>>(categoryList);
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

        public async Task<ActionResult> PerformanceReview()
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
                var categoryList = await _appraisalCategoryService.GetAll();
                var categoryItemList = await _appraisalCategoryItemService.GetAll();
                var employeeList = await _employeeService.GetAll();
                var employee = await _employeeService.GetByEmployerIdOrEmail(authData.Emp_No);
                var employeeAppraisal = await _employeeAppraisalService.GetByProcessor(authData.Emp_No);
                var appraisalItems = await _appraisalItemService.GetAll();

                appraisalViewModel.AppraisalRatings = _mapper.Map<IEnumerable<AppraisalRatingModel>>(ratingList);
                appraisalViewModel.AppraisalCategories = _mapper.Map<IEnumerable<AppraisalCategoryModel>>(categoryList);
                appraisalViewModel.AppraisalCategoryItems = _mapper.Map<IEnumerable<AppraisalCategoryItemModel>>(categoryItemList);
                appraisalViewModel.EmployeeList = _mapper.Map<IEnumerable<EmployeeModel>>(employeeList);
                appraisalViewModel.Employee = _mapper.Map<EmployeeModel>(employee);
                appraisalViewModel.EmployeeAppraisal = _mapper.Map<IEnumerable<EmployeeAppraisalModel>>(employeeAppraisal);
                appraisalViewModel.AppraisalItem = _mapper.Map<IEnumerable<AppraisalItemModel>>(appraisalItems);

                return View(appraisalViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
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
                    if(appraisalPeriod == null)
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
                            NextRatingManagerName = approverConfig.Employee.LastName + approverConfig.Employee.FirstName,
                            AppraisalPeriodId = appraisalPeriod.Id,
                            Id = Guid.NewGuid(),
                            CreatedDate = DateTime.Now,
                        };

                        var appraisalItemList = new List<AppraisalItem>();

                        foreach (var item in catItemWeight)
                        {
                            Guid category = Guid.Parse(item.Split("/")[0]);
                            Guid categoryItem = Guid.Parse(item.Split("/")[1]);
                            Guid rating = Guid.Parse(item.Split("/")[2]);

                            appraisalItemList.Add(new AppraisalItem() { AppraisalCategoryId = category, AppraisalCategoryItemId = categoryItem, AppraisalRatingId = rating, EmployeeAppraisalId = employeeAppraisal.Id, Id = Guid.NewGuid(), CreatedDate = DateTime.Now });
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
                            CreatedDate = DateTime.Now
                        };

                        var appraisalItemList = new List<AppraisalItem>();

                        foreach (var item in catItemWeight)
                        {
                            Guid category = Guid.Parse(item.Split("/")[0]);
                            Guid categoryItem = Guid.Parse(item.Split("/")[1]);
                            Guid rating = Guid.Parse(item.Split("/")[2]);

                            appraisalItemList.Add(new AppraisalItem() { AppraisalCategoryId = category, AppraisalCategoryItemId = categoryItem, AppraisalRatingId = rating, EmployeeAppraisalId = employeeAppraisal.Id, Id = Guid.NewGuid(), CreatedDate = DateTime.Now });
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
                        empAppraisal = await _employeeAppraisalService.GetById(appraisalId);
                    }

                    return Json(new
                    {
                        status = (response != null) ? true : false,
                        data = new { detail = response, level = board.ApprovalLevel, serviceId = board.ServiceId, unsigned = (unsignedBoard != null) ? true: false, apparaisalid = appraisalId, strenght = empAppraisal.Strenght, weekness = empAppraisal.Weekness, development = empAppraisal.Development, counselling = empAppraisal.Counselling, redeployment = empAppraisal.Redeployment, action = empAppraisal.DisciplinaryAction, training = empAppraisal.Training, promotion = empAppraisal.Promotion, others = empAppraisal.OtherDetail}
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
        public async Task<ActionResult> SignOffAppraisal(Guid appraisalId)
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

                    var response = await _approvalBoardService.SignOffAppraisal(appraisalId);

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