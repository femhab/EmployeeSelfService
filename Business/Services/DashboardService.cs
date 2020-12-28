using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeService _employeeService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly ILeaveService _leaveService;
        private readonly ILeaveTypeService _leaveTypeService;

        public DashboardService(IUnitOfWork unitOfWork, IEmployeeService employeeService, IApprovalBoardService approvalBoardService, ILeaveService leaveService, ILeaveTypeService leaveTypeService)
        {
            _unitOfWork = unitOfWork;
            _employeeService = employeeService;
            _approvalBoardService = approvalBoardService;
            _leaveService = leaveService;
            _leaveTypeService = leaveTypeService;
        }

        public async Task<DashboardResponseModel> GetDashboard(Guid employeeId)
        {
            var profile = await _employeeService.GetById(employeeId);

            DashboardResponseModel dashboardResponseModel = new DashboardResponseModel();
            var newEmployees = await _employeeService.GetAll(predicate: c => c.CreatedDate.Month == DateTime.Now.Month);
            dashboardResponseModel.NewUserCount = newEmployees.Count();

            var approvals = await _approvalBoardService.GetByProcessor(employeeId);
            dashboardResponseModel.ApprovalDone = approvals.Where(x => x.Status == ApprovalStatus.Approved).Count();
            dashboardResponseModel.ApprovalPending = approvals.Where(x => x.Status == ApprovalStatus.Pending).Count();

            if(profile.DepartmentId != null)
            {
                var departmentleave = await _leaveService.GetByDepartment(profile.DepartmentId.Value);

                dashboardResponseModel.LeaveApprovedInDepartment = departmentleave.Where(x => (x.Status == ApprovalStatus.Approved || x.LeaveStatus == LeaveStatus.Approved) && x.DateFrom.Month == DateTime.Now.Month).Count();

                dashboardResponseModel.LeaveOngoingInDepartment = departmentleave.Where(x => x.LeaveStatus == LeaveStatus.OnLeave).Count();
            }

            var leaveType = (await _leaveTypeService.GetAll(predicate: c => c.GradeLevelId == profile.GradeLevelId && c.Class == LeaveClass.Annual)).FirstOrDefault();
            if(leaveType != null)
            {
                dashboardResponseModel.AnnualLeaveDaysLimit = leaveType.AvailableDays;
            }

            var thisYearLeave = (await _leaveService.GetAll(predicate: c => c.EmployeeId == employeeId && c.CreatedDate.Year == DateTime.Now.Year && c.LeaveType.Class == LeaveClass.Annual && (c.Status == ApprovalStatus.Approved || c.LeaveStatus == LeaveStatus.Approved || c.LeaveStatus == LeaveStatus.Completed || c.LeaveStatus == LeaveStatus.OnLeave))).FirstOrDefault();
            if(thisYearLeave != null)
            {
                dashboardResponseModel.LeaveDaysEligible = thisYearLeave.RemainingDays.Value;
            }

            return dashboardResponseModel;
        }
    }
}
