using Data.Entities.Common;

namespace Data.Entities
{
    public class JobChangeReason: BaseObject
    {
        public string ChangeReasonCode { get; set; }
        public string PensionDocumentsID { get; set; }
    }
}
