using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMapper _mapper;

        public TrainingController(IMapper mapper, ITrainingService trainingService, ITrainingNominationService trainingNominationService, IApprovalBoardService approvalBoardService)
        {
            _mapper = mapper;
            _trainingService = trainingService;
            _trainingNominationService = trainingNominationService;
            _approvalBoardService = approvalBoardService;
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

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ApplyTraining(Guid? nominationId, bool? isSchedule, string trainingTopic, int? year, string startDate, string endDate, string otherDetails)
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

                    var trainingApplication = new Training();
                    trainingApplication.EmployeeId = authData.Id;
                    trainingApplication.Emp_No = authData.Emp_No;
                    trainingApplication.CreatedDate = DateTime.Now;
                    trainingApplication.Id = Guid.NewGuid();
                    trainingApplication.IsScheduled = isSchedule.Value;
                    if (nominationId != null)
                    {
                        var nominationDetail = await _trainingNominationService.GetById(nominationId.Value);
                        trainingApplication.StartDate = nominationDetail.TrainingCalender.StartDate.Value;
                        trainingApplication.EndDate = nominationDetail.TrainingCalender.EndDate.Value;
                        trainingApplication.TrainingTopic = nominationDetail.TrainingCalender.Topic.Title;
                        trainingApplication.TrainingYear = nominationDetail.TrainingCalender.TrainingYear.Value;
                    }
                    else
                    {
                        var startingDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var endingDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        trainingApplication.StartDate = startingDate;
                        trainingApplication.EndDate = endingDate;
                        trainingApplication.TrainingTopic = trainingTopic;
                        trainingApplication.TrainingYear = year.Value;
                        trainingApplication.OtherDetails = otherDetails;
                    }
                    var response = await _trainingService.Create(trainingApplication, nominationId);
                     if (nominationId != null) 
                        await _trainingNominationService.UpdateStatus(nominationId.Value, true);
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
