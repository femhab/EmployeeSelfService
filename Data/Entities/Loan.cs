using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class Loan : BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid LoanTypeId { get; set; }
        public LoanType LoanType { get; set; }
        public DateTime StartDate { get; set; }
        public int NoOfInstallment { get; set; }
        public DateTime EndDate { get; set; }
        public decimal AmountRequested { get; set; }
        public decimal InterestRate { get; set; }
        public decimal AmountApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public decimal InstallmentAmount { get; set; }
        public string Reason { get; set; }
        public ApprovalStatus Status { get; set; }
        public LoanStatus LoanStatus { get; set; }
        public string LastApprover {get; set;}
    }
}
