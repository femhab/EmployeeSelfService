using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IAppraisalItemService _appraisalItemService;
        private readonly IDashboardService _dashboardService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DashboardController(IApprovalBoardService approvalBoardService, IMapper mapper, IDashboardService dashboardService, IAppraisalCategoryService appraisalCategoryService, IAppraisalRatingService appraisalRatingService, IEmployeeAppraisalService employeeAppraisalService, IUnitOfWork unitOfWork, IAppraisalItemService appraisalItemService)
        {
            _appraisalRatingService = appraisalRatingService;
            _approvalBoardService = approvalBoardService;
            _appraisalCategoryService = appraisalCategoryService;
            _employeeAppraisalService = employeeAppraisalService;
            _appraisalItemService = appraisalItemService;
            _dashboardService = dashboardService;
            _unitOfWork = unitOfWork;
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
        public async Task<ActionResult> ApprovalBoardAction(bool status, Level approvalLevel, Guid serviceId)
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
                    var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("leave"));
                    var response = await _approvalBoardService.ApprovalAction(authData.Id, status? ApprovalStatus.Approved: ApprovalStatus.Rejected, approvalLevel, serviceId, approvalWorkItem.Id);
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
        public async Task<ActionResult> ApprovalBoardAppraisalAction(bool status, Level approvalLevel, Guid serviceId, List<string> categoryItemUpdate, string strenght, string weekness, string counselling, string redeployment, string development, string disciplinaryAction, string training, string promotion, string otherDetail)
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
                    var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("appraisal"));
                    //if approved update catgoryitem weight
                    var updateList = new List<AppraisalItemUpdateModel>();
                    var appraisalId = new Guid();
                    foreach (var item in categoryItemUpdate)
                    {
                        if(item != null)
                        {
                            Guid empAppraisal = Guid.Parse(item.Split("/")[0]);
                            Guid categoryItem = Guid.Parse(item.Split("/")[1]);
                            Guid rating = string.IsNullOrEmpty(item.Split("/")[2]) ? Guid.Empty : Guid.Parse(item.Split("/")[2]);

                            if (rating != Guid.Empty)
                            {
                                updateList.Add(new AppraisalItemUpdateModel() { EmployeeAppraisalId = empAppraisal, CategoryItemId = categoryItem, RatingId = rating });
                            }
                            appraisalId = empAppraisal;
                        }
                    }
                    var updatedAppraisal = await _employeeAppraisalService.GetById(serviceId);
                    if(updatedAppraisal != null)
                    {
                        updatedAppraisal.Strenght = strenght;
                        updatedAppraisal.Counselling = counselling;
                        updatedAppraisal.Redeployment = redeployment;
                        updatedAppraisal.Development = development;
                        updatedAppraisal.DisciplinaryAction = disciplinaryAction;
                        updatedAppraisal.Training = training;
                        updatedAppraisal.Promotion = promotion;
                        updatedAppraisal.OtherDetail = otherDetail;
                        updatedAppraisal.Weekness = weekness;
                        _unitOfWork.GetRepository<EmployeeAppraisal>().Update(updatedAppraisal);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    await _appraisalItemService.Update(updateList);
                    var response = await _approvalBoardService.ApprovalAction(authData.Id, status ? ApprovalStatus.Approved : ApprovalStatus.Rejected, approvalLevel, serviceId, approvalWorkItem.Id);
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ApprovalBoardTrainingAction(bool status, Level approvalLevel, Guid serviceId)
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
                    var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("training"));
                    var response = await _approvalBoardService.ApprovalAction(authData.Id, status ? ApprovalStatus.Approved : ApprovalStatus.Rejected, approvalLevel, serviceId, approvalWorkItem.Id);
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ApprovalBoardLoanAction(bool status, Level approvalLevel, Guid serviceId)
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
                    var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("loan"));
                    var response = await _approvalBoardService.ApprovalAction(authData.Id, status ? ApprovalStatus.Approved : ApprovalStatus.Rejected, approvalLevel, serviceId, approvalWorkItem.Id);
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ApprovalBoardAdvanceAction(bool status, Level approvalLevel, Guid serviceId)
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
                    var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("payment advance"));
                    var response = await _approvalBoardService.ApprovalAction(authData.Id, status ? ApprovalStatus.Approved : ApprovalStatus.Rejected, approvalLevel, serviceId, approvalWorkItem.Id);
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

        public async Task<ActionResult> ApprovalBoardTransferAction(bool status, Level approvalLevel, Guid serviceId)
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
                    var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("transfer"));
                    var response = await _approvalBoardService.ApprovalAction(authData.Id, status ? ApprovalStatus.Approved : ApprovalStatus.Rejected, approvalLevel, serviceId, approvalWorkItem.Id);
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