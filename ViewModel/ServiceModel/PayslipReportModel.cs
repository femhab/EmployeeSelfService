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
        public string YTD_ITF { get; set; }
        public string YTD_CPS { get; set; }
        public string YTD_NSITF { get; set; }
        public string YTD_NHFA { get; set; }
        public string YTD_GRPLIFE { get; set; }
        public string YTD_GRATUIT { get; set; }
        public string YTD_pacmexp { get; set; }
        public string empcontribution { get; set; }
        public string Division { get; set; }
        public decimal GrossPay { get; set; }
        public DateTime Date_Time { get; set; }
        public decimal TaxablePay { get; set; }
        public decimal freePay { get; set; }
        public decimal taxPaid { get; set; }
        public decimal NetPay { get; set; }
        public string gradeCode { get; set; }
        public string accountno { get; set; }
        public string BankName { get; set; }
        public string ITF { get; set; }
        public string NHF { get; set; }
        public string NSITF { get; set; }
        public string GRATUIT { get; set; }
        public string GRPLIFE { get; set; }
        public string PACMEXP { get; set; }
        public string totalcc { get; set; }
        public string levelcode { get; set; }
        public string PFA { get; set; }
        public string pay_period { get; set; }
        public string CompDescc { get; set; }
        public string companyaddress { get; set; }
        public string EmployeeRA { get; set; }
        public string HourlyRate { get; set; }
        public decimal Rate { get; set; }
        public string OrdinaryHours { get; set; }
        public string OrdinaryDays { get; set; }
        public string employerytd { get; set; }
        public string date_birth1 { get; set; }
        public string date_Emp1 { get; set; }
        public string costcenter { get; set; }
    }

    public class EarningReportModel
    {
        public string emp_no { get; set; }
        public string earnings { get; set; }
        public string prz_payTypCode { get; set; }
        public decimal? amount { get; set; }
        public float? ytd_amount { get; set; }
        public decimal? CurrencyRate { get; set; }
    }

    public class DeductionReportModel
    {
        public string emp_no { get; set; }
        public string deductions { get; set; }
        public string amount { get; set; }
        public string YTD_Amount { get; set; }
    }
}
