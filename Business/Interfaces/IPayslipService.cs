using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Interfaces
{
    public interface IPayslipService
    {
        Task<PayslipResponseModel> GeneratePayslipData(Guid employeeId, DateTime payPeriod);
        Task<List<PayrollCalender>> PayrollCalender();
    }
}
