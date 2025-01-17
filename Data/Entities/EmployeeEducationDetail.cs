﻿using Data.Entities.Common;
using System;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class EmployeeEducationDetail: BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public string Institution { get; set; }
        public string Course { get; set; }
        public Guid EducationalLevelId { get; set; }
        public EducationalLevel EducationalLevel { get; set; }
        public Guid EducationalQualificationId { get; set; }
        public EducationalQualification EducationalQualification { get; set; }
        public Guid EducationalGradeId { get; set; }
        public EducationalGrade EducationalGrade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //list to be updated

    }
}
