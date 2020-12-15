using System;
using System.Collections.Generic;

namespace ViewModel.ResponseModel
{
    public class LeaveResponseModel: BaseResponse
    {
        public bool IsFiveYearsAnniversary { get; set; }
        public bool IsTenYearsAnniversary { get; set; }
        public IEnumerable<LeaveTypeAudit> LeaveTypeAudit { get; set; }
        public bool IsLeaveEligible { get; set; }
        public bool IsRecallEligible { get; set; }
    }

    public class LeaveTypeAudit
    {
        public Guid LeaveType { get; set; }
        public int NoDaysRemaining { get; set; }
    }
}
