using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class Department: BaseObject
    {
        public string DeptCode { get; set; }
        public string Descc { get; set; }
        public string CompanyCode { get; set; }
        public string DivisionCode { get; set; }   
        public int Slot { get; set; }
        public bool CanClearEmployeeOnExit { get; set; }
        public Guid? HOD { get; set; } //employeeId of the hod
    }
}
