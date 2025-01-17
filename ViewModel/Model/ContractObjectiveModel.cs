﻿using System;
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

    public class ContractItemModel : BaseModel
    {
        public Guid ContractObjectiveId { get; set; }
        public ContractObjectiveModel ContractObjective { get; set; }
        public string SmartObjective { get; set; }
        public string EvaluationCiteria { get; set; }
        public DateTime Timeline { get; set; }
        public int Weighting { get; set; }
        public int ScoreAchieved { get; set; }
        public decimal WeightedSore { get; set; } //weighiung * scoreachieved
        public string Remark { get; set; } //weighiung * scoreachieved
    }
}
