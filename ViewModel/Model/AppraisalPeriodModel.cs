using System;

namespace ViewModel.Model
{
    public class AppraisalPeriodModel: BaseModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
