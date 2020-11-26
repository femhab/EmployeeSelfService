using AutoMapper;
using Business.Interfaces;
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
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public AppraisalController(IAppraisalRatingService appraisalRatingService, IAppraisalCategoryService appraisalCategoryService, IAppraisalCategoryItemService appraisalCategoryItemService, IEmployeeService employeeService, IMapper mapper)
        {
            _appraisalRatingService = appraisalRatingService;
            _appraisalCategoryService = appraisalCategoryService;
            _appraisalCategoryItemService = appraisalCategoryItemService;
            _employeeService = employeeService;
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
                appraisalViewModel.AppraisalRatings = _mapper.Map<IEnumerable<AppraisalRatingModel>>(ratingList);
                appraisalViewModel.AppraisalCategories = _mapper.Map<IEnumerable<AppraisalCategoryModel>>(categoryList);
                appraisalViewModel.AppraisalCategoryItems = _mapper.Map<IEnumerable<AppraisalCategoryItemModel>>(categoryItemList);
                appraisalViewModel.EmployeeList = _mapper.Map<IEnumerable<EmployeeModel>>(employeeList);

                return View(appraisalViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }
    }
}