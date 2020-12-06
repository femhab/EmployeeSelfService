using System;
using System.Collections.Generic;
using ViewModel.Enumeration;

namespace ViewModel.Model
{
    public class LoanModel: BaseModel
    {
        public Guid EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid LoanTypeId { get; set; }
        public LoanTypeModel LoanType { get; set; }
        public DateTime StartDate { get; set; }
        public int NoOfInstallment { get; set; }
        public DateTime EndDate { get; set; }
        public decimal AmountRequested { get; set; }
        public decimal InterestRate { get; set; }
        public decimal AmountApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public decimal InstallmentAmount { get; set; }
        public string Reason { get; set; }
        public ApprovalStatusEnum Status { get; set; }
        public LoanStatusEnum LoanStatus { get; set; }
        public string LastApprover { get; set; }
    }

    public class LoanViewModel
    {
        public EmployeeModel Employee { get; set; }
        public IEnumerable<LoanTypeModel> LoanType { get; set; }
        public IEnumerable<LoanModel> LoanTaken { get; set; }
    }
}
