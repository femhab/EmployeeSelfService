using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class EmployeeNOKDetail: BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public Guid RelationshipId { get; set; }
        public Relationship Relationship { get; set; }
        public ApprovalStatus Status { get; set; }
    }
}
