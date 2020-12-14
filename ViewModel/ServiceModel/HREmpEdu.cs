using System;

namespace ViewModel.ServiceModel
{
    public class HREmpEdu: BaseServiceModel
    {
        public int HREmpEduId { get; set; }
        public string Emp_No { get; set; }
        public string EduclLevelCode { get; set; }
        public string EduTypCode { get; set; }
        public string SchoolCode { get; set; }
        public DateTime grad_year { get; set; }
        public string Note { get; set; }
        public string DegreeCode { get; set; }
        public string EducDscCode { get; set; }
        public string GradeCode { get; set; }
        public string CountryCode { get; set; }
    }
}
