using AutoMapper;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Model;
using Web.Helper.JWT;

namespace Web.Controllers
{
    public class TrainingController : BaseController
    {
        private readonly ITrainingService _trainingService;
        private readonly IMapper _mapper;

        public TrainingController(IMapper mapper, ITrainingService trainingService)
        {
            _mapper = mapper;
            _trainingService = trainingService;
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

            trainingViewModel = _mapper.Map<TrainingViewModel>(authData);
            trainingViewModel.TrainingTopics = _mapper.Map<IEnumerable<TrainingTopicModel>>(trainingTopics);

            return View(trainingViewModel);
        }

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ApplyTraining(bool isSchedule, dynamic trainingTopic, int year, string startDate, string EndDate, string otherDetails)
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

                    
                   
                    return Json(new
                    {
                        //status = response.Status,
                        //message = response.Message
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
