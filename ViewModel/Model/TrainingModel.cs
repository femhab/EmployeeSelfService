using System;
using System.Collections.Generic;

namespace ViewModel.Model
{
    public class TrainingModel: BaseModel
    {
        public Guid EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public string TrainingTopic { get; set; }
        public int TrainingYear { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsScheduled { get; set; }
    }

    public class TrainingViewModel: AuthDataModel
    {
        public IEnumerable<TrainingTopicModel> TrainingTopics { get; set; }
    }
}
