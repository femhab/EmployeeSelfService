using AutoMapper;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Model;
using Web.Helper.JWT;

namespace Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IMapper _mapper;

        public DashboardController(IApprovalBoardService approvalBoardService, IMapper mapper)
        {
            _approvalBoardService = approvalBoardService;
            _mapper = mapper;
        }

        [Route("Dashboard")]
        public IActionResult Employee()
        {
            return View();
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

            approvalBoardViewModel.ApprovalBoard = _mapper.Map<IEnumerable<ApprovalBaordModel>>(approvalBoard);

            return View(approvalBoardViewModel);
        }
    }
}