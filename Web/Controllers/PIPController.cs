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
    public class PIPController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IPIPService _pipService;

        public PIPController(IMapper mapper, IEmployeeService employeeService, IPIPService pipService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _pipService = pipService;
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

                PIPViewModel pipViewModel = new PIPViewModel();
                var employeeList = await _employeeService.GetLineEmployee(authData.Emp_No, authData.DepartmentId.Value);

                pipViewModel = _mapper.Map<PIPViewModel>(authData);
                pipViewModel.Employee = _mapper.Map<IEnumerable<EmployeeModel>>(employeeList);

                return View(pipViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        public async Task<ActionResult> ViewPIP()
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                PIPViewModel pipViewModel = new PIPViewModel();
                var pipQuery = await _pipService.GetByEmployee(authData.Emp_No);

                pipViewModel = _mapper.Map<PIPViewModel>(authData);
                pipViewModel.PIPQuery = _mapper.Map<IEnumerable<PIPModel>>(pipQuery);

                return View(pipViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddPIP(string empNo, string subject, string message, string date)
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
                    var reviewDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var employee = await _employeeService.GetByEmployerIdOrEmail(empNo);
                    var model = new PIP()
                    {
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        LineManager = authData.Emp_No,
                        PIPSubject = subject.Replace("<p>", string.Empty).Replace("</p>", string.Empty).Replace("<br>", string.Empty).Replace("</br>", string.Empty),
                        PIPMessage = message.Replace("<p>", string.Empty).Replace("</p>", string.Empty).Replace("<br>", string.Empty).Replace("</br>", string.Empty),
                        DateOfReview = reviewDate,
                        Emp_No = empNo,
                        EmployeeId = employee.Id
                    };

                    var response = await _pipService.CreatePIP(model);

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
        public async Task<ActionResult> GetPipById(Guid pipId)
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

                    var pipQuery = await _pipService.GetById(pipId);
                    var response = await _pipService.GetByPIP(pipId);
                    var signoff = false;
                    if(authData.Emp_No.ToLower() == pipQuery.LineManager.ToLower())
                    {
                        signoff = true;
                    }

                    return Json(new
                    {
                        status = (response != null) ? true : false,
                        data = new { pipitem = response, pip = pipQuery, isLineManager = signoff }
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
        public async Task<ActionResult> UpdatePIP(Guid pipId, string comment)
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

                    var model = new PIPItem() { Comment = comment.Replace("<p>", string.Empty).Replace("</p>", string.Empty).Replace("<br>", string.Empty).Replace("</br>", string.Empty), PublishBy = authData.Emp_No, PIPId = pipId, Id = Guid.NewGuid(), CreatedDate = DateTime.Now };



                    var response = await _pipService.CreatePIPItem(model);
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
