using System;
using ViewModel.Enumeration;

namespace ViewModel.Model
{
    public class EmployeeFamilyDependentModel: BaseModel
    {
        public Guid EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public Guid RelationshipId { get; set; }
        public RelationshipModel Relationship { get; set; }
        public ApprovalStatusEnum Status { get; set; }
        public string PictureUrl { get; set; }
    }
}
