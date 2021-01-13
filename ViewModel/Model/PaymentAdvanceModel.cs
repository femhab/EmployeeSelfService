using System;
using System.Collections.Generic;
using ViewModel.Enumeration;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace ViewModel.Model
{
    public class PaymentAdvanceModel: BaseModel
    {
        public Guid EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public DateTime TargetDate { get; set; }
        public decimal Amount { get; set; } //max 100%
        public ApprovalStatusEnum Status { get; set; }
        public string LastProcessor { get; set; }
    }

    public class PayrollViewModel: AuthDataModel
    {
        public PaymentAdvanceResponseModel Eligibility { get; set; }
        public IEnumerable<PaymentAdvanceModel> PaymentAdvance { get; set; }
        public IEnumerable<PayrollCalender> PayrollCalender { get; set; }
        public PayslipResponseModel PayslipResponse { get; set; }
        public EmployeeModel Employee { get; set; }
    }
}
