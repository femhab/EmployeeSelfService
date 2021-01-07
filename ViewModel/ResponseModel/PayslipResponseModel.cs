using System.Collections.Generic;
using ViewModel.ServiceModel;

namespace ViewModel.ResponseModel
{
    public class PayslipResponseModel
    {
        public BasicReportModel BasicReport { get; set; }
        public List<EarningReportModel> EarningReport { get; set; }
        public List<DeductionReportModel> DeductionReport { get; set; }
    }
}
