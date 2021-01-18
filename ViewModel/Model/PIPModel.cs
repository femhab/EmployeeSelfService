using System;
using System.Collections.Generic;

namespace ViewModel.Model
{
    public class PIPModel: BaseModel
    {
        public Guid EmployeeId { get; set; } //employeeId 
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public string PIPSubject { get; set; }
        public string PIPMessage { get; set; }
        public DateTime DateOfReview { get; set; }
        public string LineManager { get; set; }
        public bool IsSignedOff { get; set; }
    }

    public class PIPItemModel : BaseModel
    {
        public Guid PIPId { get; set; }
        public PIPModel PIP { get; set; }
        public string Comment { get; set; }
        public string PublishBy { get; set; }
    }

    public class PIPViewModel: AuthDataModel
    {
        public IEnumerable<EmployeeModel> Employee { get; set; }
        public IEnumerable<PIPModel> PIPQuery { get; set; }
        public IEnumerable<PIPItemModel> PIPQueryItem { get; set; }
        public bool IsLineManager { get; set; }
    }
}
