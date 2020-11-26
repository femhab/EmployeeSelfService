using System;

namespace ViewModel.ServiceModel
{
    public class HREmpMst: BaseServiceModel
    {
        public string Emp_No { get; set; }
        public string GradeCode { get; set; }
        public string LevelCode { get; set; }
        public string DivisionCode { get; set; }
        public string UnitCode { get; set; }
        public string DeptCode { get; set; }
        public string TypeCode { get; set; }
        public string MaritalCode { get; set; }
        public string BloodCode { get; set; }
        public string GenoTypeCode { get; set; }
        public string med_limit { get; set; }
        public string HospitalCode { get; set; }
        public string DepCode { get; set; }
        public string Gender { get; set; }
        public string FSCNo { get; set; }
        public string establishNo { get; set; }
        public string stateHouseNo { get; set; }
        public string ZoneCode { get; set; }
        public string WorkStateCode { get; set; }
        public string GradeCode2 { get; set; }
        public string LevelCode2 { get; set; }
        public string TitleCode { get; set; }
        public string LocationCode { get; set; }
        public string CourtesyCode { get; set; }
        public string StatusCode { get; set; }
        public DateTime? date_Emp { get; set; }
        public DateTime? date_birth { get; set; }
        public DateTime? date_conf { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? preAppDate { get; set; }
        public DateTime? proRetireDate { get; set; }
    }
}
