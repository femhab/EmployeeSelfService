using Data.Entities.Common;
using System;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class EmployeeEducationDetail: BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        //list to be updated

    }
}
