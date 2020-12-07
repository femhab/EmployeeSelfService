using System;

namespace ViewModel.Model
{
    public class EmployeeEducationDetailModel: BaseModel
    {
        public Guid EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public string Institution { get; set; }
        public string Course { get; set; }
        public Guid EducationalLevelId { get; set; }
        public EducationalLevelModel EducationalLevel { get; set; }
        public Guid EducationalQualificationId { get; set; }
        public EducationalQualificationModel EducationalQualification { get; set; }
        public Guid EducationalGradeId { get; set; }
        public EducationalGradeModel EducationalGrade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
