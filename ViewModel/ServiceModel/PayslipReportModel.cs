using System;

namespace ViewModel.ServiceModel
{
    public class BasicReportModel
    {
        public string emp_no { get; set; }
        public string emp_name { get; set; }
        public string Department { get; set; }
        public string payGroup { get; set; }
        public string jobTitle { get; set; }
        public string JobTitle { get; set; }
        public string taxstatus { get; set; }
        public decimal YTD_GrossPay { get; set; }
        public decimal YTD_taxable { get; set; }
        public decimal YTD_freePay { get; set; }
        public decimal YTD_taxPaid { get; set; }
        public decimal YTD_netPay { get; set; }
        public decimal YTD_ITF { get; set; }
        public decimal YTD_CPS { get; set; }
        public decimal YTD_NSITF { get; set; }
        public decimal YTD_NHFA { get; set; }
        public decimal YTD_GRPLIFE { get; set; }
        public decimal YTD_GRATUIT { get; set; }
        public decimal YTD_pacmexp { get; set; }
        public decimal empcontribution { get; set; }
        //public string Division { get; set; }
        //public decimal GrossPay { get; set; }
        //public DateTime Date_Time { get; set; }
        //public decimal TaxablePay { get; set; }
        //public decimal freePay { get; set; }
        //public decimal taxPaid { get; set; }
        //public decimal NetPay { get; set; }
        //public string gradeCode { get; set; }
        //public string accountno { get; set; }
        //public string BankName { get; set; }
        //public decimal ITF { get; set; }
        //public decimal NHF { get; set; }
        //public decimal NSITF { get; set; }
        //public decimal GRATUIT { get; set; }
        //public decimal GRPLIFE { get; set; }
        //public decimal PACMEXP { get; set; }
        //public decimal totalcc { get; set; }
        //public string levelcode { get; set; }
        //public string PFA { get; set; }
        //public DateTime pay_period { get; set; }
        //public string CompDescc { get; set; }
        //public string companyaddress { get; set; }
        //public string EmployeeRA { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal Rate { get; set; }
        public decimal OrdinaryHours { get; set; }
        public decimal OrdinaryDays { get; set; }
        public decimal employerytd { get; set; }
        public DateTime date_birth1 { get; set; }
        public DateTime date_Emp1 { get; set; }
        public string costcenter { get; set; }
    }

    public class EarningReportModel
    {
        public string emp_no { get; set; }
        public string earnings { get; set; }
        public string prz_payTypCode { get; set; }
        public decimal amount { get; set; }
        public float ytd_amount { get; set; }
        public decimal CurrencyRate { get; set; }
    }

    public class DeductionReportModel
    {
        public string emp_no { get; set; }
        public string deductions { get; set; }
        public decimal amount { get; set; }
        public float YTD_Amount { get; set; }
    }
}
