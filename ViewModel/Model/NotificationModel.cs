using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.Model
{
    public class NotificationModel: BaseModel
    {
        public Guid? EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsGeneral { get; set; }
    }

    public class NotificationViewModel: AuthDataModel
    {
        public IEnumerable<NotificationModel> Notification { get; set; }
    }
}
