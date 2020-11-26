using System;

namespace ViewModel.ServiceModel
{
    public class AppraisalPeriod
    {
        public int AppraisalPeriodID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
