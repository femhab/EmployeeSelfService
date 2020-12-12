using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        private readonly IAppraisalPeriodService _appraisalPeriodService;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public AppraisalController(IAppraisalRatingService appraisalRatingService, IAppraisalCategoryService appraisalCategoryService, IAppraisalCategoryItemService appraisalCategoryItemService, IEmployeeService employeeService, IMapper mapper, IEmployeeApprovalConfigService employeeApprovalConfigService, IAppraisalPeriodService appraisalPeriodService)
        {
            _appraisalRatingService = appraisalRatingService;
            _appraisalCategoryService = appraisalCategoryService;
            _appraisalCategoryItemService = appraisalCategoryItemService;
            _appraisalPeriodService = appraisalPeriodService;
            _employeeService = employeeService;
            _employeeApprovalConfigService = employeeApprovalConfigService;
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

                appraisalViewModel = _mapper.Map<AppraisalViewModel>(authData);
                appraisalViewModel.AppraisalRatings = _mapper.Map<IEnumerable<AppraisalRatingModel>>(ratingList);
                appraisalViewModel.AppraisalCategories = _mapper.Map<IEnumerable<AppraisalCategoryModel>>(categoryList);
                appraisalViewModel.AppraisalCategoryItems = _mapper.Map<IEnumerable<AppraisalCategoryItemModel>>(categoryItemList);
                appraisalViewModel.EmployeeList = _mapper.Map<IEnumerable<EmployeeModel>>(employeeList);
                appraisalViewModel.Employee = _mapper.Map<EmployeeModel>(employee);

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
                appraisalViewModel.AppraisalRatings = _mapper.Map<IEnumerable<AppraisalRatingModel>>(ratingList);
                appraisalViewModel.AppraisalCategories = _mapper.Map<IEnumerable<AppraisalCategoryModel>>(categoryList);
                appraisalViewModel.AppraisalCategoryItems = _mapper.Map<IEnumerable<AppraisalCategoryItemModel>>(categoryItemList);
                appraisalViewModel.EmployeeList = _mapper.Map<IEnumerable<EmployeeModel>>(employeeList);
                appraisalViewModel.Employee = _mapper.Map<EmployeeModel>(employee);

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
                    var appraisalPeriod = await _appraisalPeriodService.GetActivePeriod();

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

                        return Json(new
                        {
                            status = true,
                            message = "Appraisal created successfully"
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
    }
}