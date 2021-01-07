using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Model;
using Web.Helper.JWT;

namespace Web.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public NotificationController(INotificationService notificationService, IMapper mapper)
        {
            _notificationService = notificationService;
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
                //await GeneratePayslipData();
                NotificationViewModel notificationViewModel = new NotificationViewModel();
                var notification = await _notificationService.GetByEmployee(authData.Id);

                notificationViewModel.Notification = _mapper.Map<IEnumerable<NotificationModel>>(notification);

                return View(notificationViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }
    }
}