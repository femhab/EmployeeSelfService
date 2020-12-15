using System.Collections.Generic;

namespace ViewModel.Model
{
    public class AdminViewModel: AuthDataModel
    {
        public IEnumerable<EmployeeModel> EmployeeList { get; set; }
        public IEnumerable<RoleModel> Roles { get; set; }
    }
}
