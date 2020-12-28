using AutoMapper;
using Business.Interfaces;
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
    public class DashboardController : BaseController
    {
        private readonly IAppraisalRatingService _appraisalRatingService;
        private readonly IAppraisalCategoryService _appraisalCategoryService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IEmployeeAppraisalService _employeeAppraisalService;
        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;

        public DashboardController(IApprovalBoardService approvalBoardService, IMapper mapper, IDashboardService dashboardService, IAppraisalCategoryService appraisalCategoryService, IAppraisalRatingService appraisalRatingService, IEmployeeAppraisalService employeeAppraisalService)
        {
            _appraisalRatingService = appraisalRatingService;
            _approvalBoardService = approvalBoardService;
            _appraisalCategoryService = appraisalCategoryService;
            _employeeAppraisalService = employeeAppraisalService;
            _dashboardService = dashboardService;
            _mapper = mapper;
        }

        [Route("Dashboard")]
        public async Task<ActionResult> Employee()
        {
            var authData = JwtHelper.GetAuthData(Request);
            if (authData == null)
            {
                return RedirectToAction("Signout", "Employee");
            }

            DashboardModel dashModel = new DashboardModel();
            var dashbaordStatistics = await _dashboardService.GetDashboard(authData.Id);

            dashModel = _mapper.Map<DashboardModel>(authData);
            dashModel.NewUserCount = dashbaordStatistics.NewUserCount;
            dashModel.ApprovalPending = dashbaordStatistics.ApprovalPending;
            dashModel.ApprovalDone = dashbaordStatistics.ApprovalDone;
            dashModel.LeaveApprovedInDepartment = dashbaordStatistics.LeaveApprovedInDepartment;
            dashModel.LeaveOngoingInDepartment = dashbaordStatistics.LeaveOngoingInDepartment;
            dashModel.AnnualLeaveDaysLimit = dashbaordStatistics.AnnualLeaveDaysLimit;
            dashModel.LeaveDaysEligible = dashbaordStatistics.LeaveDaysEligible;

            return View(dashModel);
        }

        [Route("Approval-Board")]
        public async Task<ActionResult> Approval()
        {
            var authData = JwtHelper.GetAuthData(Request);
            if (authData == null)
            {
                return RedirectToAction("Signout", "Employee");
            }

            ApprovalBoardViewModel approvalBoardViewModel = new ApprovalBoardViewModel();
            var approvalBoard = await _approvalBoardService.GetByProcessor(authData.Id);
            var categoryList = await _appraisalCategoryService.GetAll();
            var ratingList = await _appraisalRatingService.GetAll();

            approvalBoardViewModel = _mapper.Map<ApprovalBoardViewModel>(authData);
            approvalBoardViewModel.ApprovalBoard = _mapper.Map<IEnumerable<ApprovalBaordModel>>(approvalBoard);
            approvalBoardViewModel.AppraisalCategories = _mapper.Map<IEnumerable<AppraisalCategoryModel>>(categoryList);
            approvalBoardViewModel.AppraisalRatings = _mapper.Map<IEnumerable<AppraisalRatingModel>>(ratingList);

            return View(approvalBoardViewModel);
        }

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ApprovalBoardAction(Guid ProcessorId, ApprovalStatus status, Level approvalLevel, Guid serviceId, Guid approvalWorkItemId)
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
                    var response = await _approvalBoardService.ApprovalAction(ProcessorId, status, approvalLevel, serviceId, approvalWorkItemId);
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
    }
}