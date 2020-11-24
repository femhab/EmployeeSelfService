namespace ViewModel.Model
{
    public class DepartmentModel: BaseModel
    {
        public string DeptCode { get; set; }
        public string Descc { get; set; }
        public string CompanyCode { get; set; }
        public string DivisionCode { get; set; }
        public int Slot { get; set; }
    }
}
