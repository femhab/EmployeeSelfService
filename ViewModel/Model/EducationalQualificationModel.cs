namespace ViewModel.Model
{
    public class EducationalQualificationModel: BaseModel
    {
        public string Code { get; set; } //BSc, MSc
        public string Description { get; set; } //Bachelor, Masters
        public string EducationalLevelCode { get; set; }
    }
}
