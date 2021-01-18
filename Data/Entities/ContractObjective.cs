using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class ContractObjective: BaseObject
    {
        public Guid EmployeeId { get; set; } //EmployeeId
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public decimal TotalWeightedSore { get; set; }
        public bool IsSignedOff { get; set; }
        public DateTime? SignedOffDate { get; set; }
        public string LineManager { get; set; }
    }

    public class ContractItem: BaseObject
    {
        public Guid ContractObjectiveId { get; set; }
        public ContractObjective ContractObjective { get; set; }
        public string SmartObjective { get; set; }
        public string EvaluationCiteria { get; set; }
        public DateTime Timeline { get; set; }
        public int Weighting { get; set; }
        public int ScoreAchieved { get; set; }
        public decimal WeightedSore { get; set; } //weighiung * scoreachieved
        public string Remark { get; set; } //weighiung * scoreachieved
    }
}
