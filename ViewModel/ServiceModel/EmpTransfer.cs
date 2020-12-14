using System;

namespace ViewModel.ServiceModel
{
    public class EmpTransfer: BaseServiceModel
    {
        public int EmpTransferId { get; set; } 
        public string OldDeptCode { get; set; }
        public string NewDeptCode { get; set; }
        public string Reason { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }
}
