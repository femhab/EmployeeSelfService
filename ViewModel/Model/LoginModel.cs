using System;
using ViewModel.Enumeration;

namespace ViewModel.Model
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthDataModel
    {
        public Guid Id { get; set; }
        public string IdentityId { get; set; }
        public string Emp_No { get; set; }
        public string Email { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid GradeLevelId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AccessTypeEnum AccessType { get; set; }
    }

    public class ApiTokenModel : ApiResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class ApiResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
