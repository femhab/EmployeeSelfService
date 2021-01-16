using Microsoft.AspNetCore.Http;
using System;
using ViewModel.Enumeration;

namespace ViewModel.Model
{
    public class EmployeeNOKDetailModel: BaseModel
    {
        public Guid EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public Guid RelationshipId { get; set; }
        public RelationshipModel Relationship { get; set; }
        public ApprovalStatusEnum Status { get; set; }
        public bool IsEmergencyContact { get; set; }
        public string PictureUrl { get; set; }
    }

    public class NokRequestModel
    {
        public string NokFirstName { get; set; }
        public string NokLastName { get; set; }
        public string NokEmail { get; set; }
        public string NokPhonenumber { get; set; }
        public Guid nokRelationShipId { get; set; }
        public string NokAddress { get; set; }
        public string NokDOB { get; set; }
        public bool IsEmergency { get; set; }
        public IFormFile NokPic { get; set; }
    }
}
