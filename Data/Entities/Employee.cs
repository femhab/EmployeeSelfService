using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class Employee : BaseObject
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string Emp_No { get; set; }
        public Guid GradeLevelId { get; set; }
        public GradeLevel GradeLevel { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public AccessType AccessType { get; set; }
    }
}
