using Data.Entities.Common;
using System;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class EmployeeEducationDetail: BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public Guid EducationalLevelId { get; set; }
        public EducationalLevel EducationalLevel { get; set; }
        public Guid EducationalQualificationId { get; set; }
        public EducationalQualification EducationalQualification { get; set; }
        public Guid EducationalGradeId { get; set; }
        public EducationalGrade EducationalGrade { get; set; }
        //list to be updated

    }
}
