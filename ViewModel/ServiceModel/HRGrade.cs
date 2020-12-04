using System;

namespace ViewModel.ServiceModel
{
    public class HRGrade: BaseServiceModel
    {
        public string GradeCode { get; set; }
        public string descc { get; set; }
        public decimal slot { get; set; }
        public string UserName { get; set; }
        public DateTime TransacDate { get; set; }
    }
}
