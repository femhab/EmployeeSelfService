using System;

namespace ViewModel.ServiceModel
{
    public class prp_calend: BaseServiceModel
    {
        public string pay_period { get; set; }
        public string prz_entityCode { get; set; }
        public string prz_paygrpCode { get; set; }
        public string prz_taxYrCode { get; set; }
    }

    public class PayrollCalender 
    {
        public DateTime PayPeriod { get; set; }
        public string PayrollEntityCode { get; set; }
        public string PayrollGroupCode { get; set; }
        public string TaxYearCode { get; set; }
    }
}
