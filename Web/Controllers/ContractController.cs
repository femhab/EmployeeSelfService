using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ViewModel.Model;
using Web.Helper.JWT;

namespace Web.Controllers
{
    public class ContractController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IContractService _contractService;
        private readonly IMapper _mapper;

        public ContractController(IEmployeeService employeeService, IMapper mapper, IContractService contractService)
        {
            _employeeService = employeeService;
            _contractService = contractService;
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

                ContractViewModel contractViewModel = new ContractViewModel();
                var empList = await _employeeService.GetContractTargetedEmployee(authData.Emp_No);
                var contract = await _contractService.GetByLineManager(authData.Emp_No);

                contractViewModel = _mapper.Map<ContractViewModel>(authData);
                contractViewModel.Employee = _mapper.Map<IEnumerable<EmployeeModel>>(empList);
                contractViewModel.ContractObjective = _mapper.Map<IEnumerable<ContractObjectiveModel>>(contract);

                return View(contractViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        public async Task<ActionResult> ViewContract()
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                ContractViewModel contractViewModel = new ContractViewModel();
                var contract = await _contractService.GetByEmployee(authData.Id);

                contractViewModel = _mapper.Map<ContractViewModel>(authData);
                contractViewModel.ContractObjective = _mapper.Map<IEnumerable<ContractObjectiveModel>>(contract);

                return View(contractViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddAppraisal(List<string> contractItem, string empNo)
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

                    var targetEmployee = await _employeeService.GetByEmployerIdOrEmail(empNo);

                    var contactObjective = new ContractObjective() { EmployeeId = targetEmployee.Id, Emp_No = empNo, Id = Guid.NewGuid(), IsSignedOff = false, TotalWeightedSore = 0, CreatedDate = DateTime.Now, LineManager = authData.Emp_No };

                    var model = new List<ContractItem>();

                    foreach(var item in contractItem)
                    {
                        string objective = item.Split("|")[0];
                        string criteria = item.Split("|")[1];
                        string timeline = item.Split("|")[2];
                        int weight = int.Parse(item.Split("|")[3]);

                        var tmeLineDate = DateTime.ParseExact(timeline, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if(tmeLineDate.Year != DateTime.Now.Year)
                        {
                            return Json(new
                            {
                                status = false,
                                message = "You cannot have a timeline for previous or next year. It has to be the current year"
                            });
                        }
                        var contractObjItem = new ContractItem() { SmartObjective = objective,  EvaluationCiteria = criteria, Weighting = weight,  Timeline = tmeLineDate, ContractObjectiveId = contactObjective.Id, CreatedDate = DateTime.Now, ScoreAchieved = 0, WeightedSore = 0, Id = Guid.NewGuid()};

                        model.Add(contractObjItem);
                    }

                    var response = await _contractService.Create(contactObjective, model);

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
        public async Task<ActionResult> GetByObjectiveId(Guid objectiveId)
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

                    var contractOjective = await _contractService.GetByObjectiveId(objectiveId);
                    var contractItem = await _contractService.GetItemByObjectiveId(objectiveId);

                    return Json(new
                    {
                        status = true,
                        data = new { objectives = contractOjective, item = contractItem }
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
