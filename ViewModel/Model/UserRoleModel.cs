using System;

namespace ViewModel.Model
{
    public class UserRoleModel: BaseModel
    {
        public Guid EmployeeId { get; set; } //employee Id
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; } //employee No
        public Guid RoleId { get; set; } //leave manager
        public RoleModel Role { get; set; }
    }
}
