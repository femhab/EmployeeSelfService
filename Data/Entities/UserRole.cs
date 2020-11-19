using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class UserRole: BaseObject
    {
        public Guid EmployeeId { get; set; } //employee Id
        public Employee Employee { get; set; }
        public string Emp_No { get; set; } //employee No
        public int RoleId { get; set; } //leave manager
        public Role Role { get; set; }
    }
}
