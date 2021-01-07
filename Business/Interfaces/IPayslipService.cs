using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IPayslipService
    {
        Task<PayslipResponseModel> GeneratePayslipData(Guid employeeId);
    }
}
