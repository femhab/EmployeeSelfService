using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class LoanService: ILoanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IApprovalBoardService _approvalBoardService;

        public LoanService(IUnitOfWork unitOfWork, IEmployeeApprovalConfigService employeeApprovalConfigService, IApprovalBoardService approvalBoardService)
        {
            _unitOfWork = unitOfWork;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _approvalBoardService = approvalBoardService;
        }

        public async Task<LoanEligibilityResponseModel> CheckEligibility(string Emp_No)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> Create(Loan model)
        {
            var check = await _unitOfWork.GetRepository<Loan>().GetFirstOrDefaultAsync(predicate: x => x.EmployeeId == model.EmployeeId && x.LoanTypeId == model.LoanTypeId && (x.LoanStatus == LoanStatus.Ongoing || x.LoanStatus == LoanStatus.Pending || x.LoanStatus == LoanStatus.Approved));
            if(check == null)
            {
                _unitOfWork.GetRepository<Loan>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                //submit for approval
                var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("loan"));
                var approvalProcessor = await _employeeApprovalConfigService.GetBy(x => x.EmployeeId == model.EmployeeId && x.ApprovalLevel == Level.FirstLevel && x.ApprovalWorkItemId == approvalWorkItem.Id);
                var enlistBoard = new ApprovalBoard()
                {
                    EmployeeId = model.EmployeeId,
                    ApprovalLevel = Level.FirstLevel,
                    Emp_No = model.Emp_No,
                    ApprovalWorkItemId = approvalWorkItem.Id,
                    ApprovalProcessorId = approvalProcessor.ProcessorIId.Value,
                    ApprovalProcessor = approvalProcessor.Processor,
                    ServiceId = model.Id,
                    Status = ApprovalStatus.Pending,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    CreatedBy = model.Emp_No
                };
                await _approvalBoardService.Create(enlistBoard);

                return new BaseResponse() { Status = true, Message = ResponseMessage.LoanCreatedSuccessfully };
            }         
            return new BaseResponse() { Status = false, Message = ResponseMessage.LoanTypeExist };
        }

        public async Task<IEnumerable<Loan>> GetByEmployee(Guid employeeId)
        {
            var data = await GetAll(x => x.EmployeeId == employeeId);
            return data;
        }

        public async Task<BaseResponse> Edit(Loan request)
        {
            var model = await GetById(request.Id);
            if (model != null && model.LoanStatus == LoanStatus.Pending )
            {
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<Loan>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.LoanProcessStarted };
        }

        public async Task<IEnumerable<Loan>> GetAll(Expression<Func<Loan, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Loan>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<Loan> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<Loan>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
