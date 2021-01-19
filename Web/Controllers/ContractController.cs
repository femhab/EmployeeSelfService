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
                var employee = await _employeeService.GetById(authData.Id);

                contractViewModel = _mapper.Map<ContractViewModel>(authData);
                contractViewModel.ContractObjective = _mapper.Map<IEnumerable<ContractObjectiveModel>>(contract);
                if(employee.GradeLevel.GradeCode == "GL08" || employee.GradeLevel.GradeCode == "GL09" || employee.GradeLevel.GradeCode == "GL10" || employee.GradeLevel.GradeCode == "GL11" || employee.GradeLevel.GradeCode == "GL12" || employee.GradeLevel.GradeCode == "GL13" || employee.GradeLevel.GradeCode == "GL14" || employee.GradeLevel.GradeCode == "GLDR")
                {
                    contractViewModel.IsContractible = true;
                }

                return View(contractViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddContract(List<string> contractItem, string empNo)
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
                    var totalweight = 0;

                    foreach(var item in contractItem)
                    {
                        string objective = item.Split("|")[0];
                        string criteria = item.Split("|")[1];
                        string timeline = item.Split("|")[2];
                        int weight = int.Parse(item.Split("|")[3]);
                        totalweight = totalweight + weight;
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

                    if(totalweight != 100)
                    {
                        return Json(new
                        {
                            status = false,
                            message = "Total weight must not be less or greater than 100"
                        });
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignOffContract(Guid contractId)
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
                    var response = await _contractService.SignOffContract(contractId);
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateContract(List<Guid> contractItems, List<int> scores, List<string> remarks)
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

                    List<ContractItem> updatedList = new List<ContractItem>();

                    int len = contractItems.Count;
                    for (int i = 0; i < len; i++)
                    {
                        var contractItem = await _contractService.GetContractItemById(contractItems[i]);
                        contractItem.ScoreAchieved = scores[i];
                        if(contractItem.ScoreAchieved > 50 || contractItem.ScoreAchieved < 0)
                        {
                            return Json(new
                            {
                                status = false,
                                message = "Score achieved can only be between 1 - 50 for each objective item"
                            });
                        }
                        contractItem.Remark = remarks[i];
                        contractItem.WeightedSore = (contractItem.ScoreAchieved * contractItem.Weighting)/100;
                        updatedList.Add(contractItem);
                    }

                    var response = await _contractService.UpdateContractItem(updatedList);

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
