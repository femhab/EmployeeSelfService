using System;
using System.Collections;
using System.Collections.Generic;

namespace ViewModel.Model
{
    public class ContractObjectiveModel: BaseModel
    {
        public Guid EmployeeId { get; set; } //EmployeeId
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public decimal TotalWeightedSore { get; set; }
        public bool IsAccessed { get; set; }
        public bool IsSignedOff { get; set; }
        public bool IsHRSignedOff { get; set; }
        public DateTime? SignedOffDate { get; set; }
        public string LineManager { get; set; }
        public string Comment { get; set; }
    }

    public class ContractViewModel: AuthDataModel
    {
        public IEnumerable<EmployeeModel> Employee { get; set; }
        public bool IsContractible { get; set; }
        public IEnumerable<ContractObjectiveModel> ContractObjective { get; set; }
    }
}
