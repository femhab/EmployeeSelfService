using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ViewModel.Model;
using Web.Helper.JWT;

namespace Web.Controllers
{
    public class TrainingController : BaseController
    {
        private readonly ITrainingService _trainingService;
        private readonly ITrainingNominationService _trainingNominationService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IMapper _mapper;

        public TrainingController(IMapper mapper, ITrainingService trainingService, ITrainingNominationService trainingNominationService, IApprovalBoardService approvalBoardService, IEmployeeApprovalConfigService employeeApprovalConfigService)
        {
            _mapper = mapper;
            _trainingService = trainingService;
            _trainingNominationService = trainingNominationService;
            _approvalBoardService = approvalBoardService;
            _employeeApprovalConfigService = employeeApprovalConfigService;
        }

        public async Task<ActionResult> Apply()
        {
            var authData = JwtHelper.GetAuthData(Request);
            if (authData == null)
            {
                return RedirectToAction("Signout", "Employee");
            }

            TrainingViewModel trainingViewModel = new TrainingViewModel();
            var trainingTopics = await _trainingService.GetAll();
            var nominations = await _trainingNominationService.GetByEmployee(authData.Emp_No);


            trainingViewModel = _mapper.Map<TrainingViewModel>(authData);
            trainingViewModel.TrainingTopics = _mapper.Map<IEnumerable<TrainingTopicModel>>(trainingTopics);
            trainingViewModel.TrainingNomination = _mapper.Map<IEnumerable<TrainingNominationModel>>(nominations);

            return View(trainingViewModel);
        }

        public class TrainingRequestModel
        {
            public Guid? NominationId { get; set; }
            public bool? IsSchedule { get; set; }
            public string TrainingTopic { get; set; }
            public int? Year { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string OtherDetails { get; set; }
            public int HoursPerDay { get; set; }
            public string Venue { get; set; }
            public decimal AmtPerHead { get; set; }
            public string Organiser { get; set; }
            public IFormFile AttachmentFile { get; set; }

        }

       // Guid? nominationId, bool? isSchedule, string trainingTopic, int? year, string startDate, string endDate, string otherDetails, int hoursPerDay, string venue, decimal amtPerHead, string organiser

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ApplyTraining(TrainingRequestModel model)
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

                    var approverConfig = await _employeeApprovalConfigService.GetByServiceLevel(authData.Id, "training", Level.FirstLevel);

                    if (approverConfig != null)
                    {
                        var trainingApplication = new Training();
                        trainingApplication.EmployeeId = authData.Id;
                        trainingApplication.Emp_No = authData.Emp_No;
                        trainingApplication.CreatedDate = DateTime.Now;
                        trainingApplication.Id = Guid.NewGuid();
                        trainingApplication.IsScheduled = model.IsSchedule.Value;
                        if (model.NominationId != null)
                        {
                            var nominationDetail = await _trainingNominationService.GetById(model.NominationId.Value);
                            trainingApplication.StartDate = nominationDetail.TrainingCalender.StartDate.Value;
                            trainingApplication.EndDate = nominationDetail.TrainingCalender.EndDate.Value;
                            trainingApplication.TrainingTopic = nominationDetail.TrainingCalender.Topic.Title;
                            trainingApplication.TrainingYear = nominationDetail.TrainingCalender.TrainingYear.Value;
                            trainingApplication.HoursPerDay = nominationDetail.TrainingCalender.HoursPerDay.Value;
                            trainingApplication.Venue = nominationDetail.TrainingCalender.Venue;
                            trainingApplication.AmtPerHead = nominationDetail.TrainingCalender.AmtPerHead.Value;
                            trainingApplication.Organizer = nominationDetail.TrainingCalender.Organiser;
                        }
                        else
                        {
                            var startingDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var endingDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            trainingApplication.StartDate = startingDate;
                            trainingApplication.EndDate = endingDate;
                            trainingApplication.TrainingTopic = model.TrainingTopic;
                            trainingApplication.TrainingYear = model.Year.Value;
                            trainingApplication.OtherDetails = model.OtherDetails;
                            trainingApplication.HoursPerDay = model.HoursPerDay;
                            trainingApplication.Venue = model.Venue;
                            trainingApplication.AmtPerHead = model.AmtPerHead;
                            trainingApplication.Organizer = model.Organiser;
                        }
                        var response = await _trainingService.Create(trainingApplication, model.NominationId);
                        if (model.NominationId != null)
                            await _trainingNominationService.UpdateStatus(model.NominationId.Value, true);
                        return Json(new
                        {
                            status = response.Status,
                            message = response.Message
                        });
                    }
                    return Json(new
                    {
                        status = false,
                        message = "You don't have approval configuration for training yet, reach out to the admin."
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
        public async Task<ActionResult> GetTrainingById(Guid trainingId)
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

                    var board = await _approvalBoardService.GetById(trainingId);

                    var response = await _trainingService.GetById(board.ServiceId);
                    return Json(new
                    {
                        status = (response != null) ? true : false,
                        data = new {  dateFrom = response.StartDate.ToString("dddd, dd MMMM yyyy"), dateTo = response.EndDate.ToString("dddd, dd MMMM yyyy"), trainingTopic = response.TrainingTopic, trainingYear = response.TrainingYear, serviceId = response.Id, level = board.ApprovalLevel }
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
