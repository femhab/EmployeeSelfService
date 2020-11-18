using System;

namespace ViewModel.ServiceModel
{
    public class HRDept: BaseServiceModel
    {
        public string DeptCode { get; set; }
        public string CompanyCode { get; set; }
        public string DivisionCode { get; set; }
        public string descc { get; set; }
        public int slot { get; set; }
        public DateTime TransacDate { get; set; }
    }
}
