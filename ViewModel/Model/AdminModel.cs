using System.Collections.Generic;

namespace ViewModel.Model
{
    public class AdminViewModel: AuthDataModel
    {
        public IEnumerable<EmployeeModel> EmployeeList { get; set; }
        public IEnumerable<RoleModel> Roles { get; set; }
        public IEnumerable<DepartmentModel> DepartmentList { get; set; }
        public IEnumerable<DepartmentModel> ClearingDepartment { get; set; }
        public IEnumerable<ApprovalWorkItemModel> ApprovalWorkItem { get; set; }
        public IEnumerable<AppraisalPeriodModel> AppraisalPeriod { get; set; }
    }
}
