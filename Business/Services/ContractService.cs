using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class ContractService: IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public ContractService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(ContractObjective model, List<ContractItem> items)
        {
            if(model != null && items.Count() > 0)
            {
                try
                {
                    _unitOfWork.GetRepository<ContractObjective>().Insert(model);
                    await _unitOfWork.SaveChangesAsync();

                    foreach (var item in items)
                    {
                        if (!string.IsNullOrEmpty(item.SmartObjective))
                        {
                            _unitOfWork.GetRepository<ContractItem>().Insert(item);
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }
                    return new BaseResponse() { Status = true, Message = ResponseMessage.ContractCreated };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> UpdateContract(Guid contractId, List<ContractItem> items)
        {
            if(contractId != null)
            {
                var contractItems = await _unitOfWork.GetRepository<ContractItem>().GetAllAsync(predicate: x => x.ContractObjectiveId == contractId);
                foreach(var item in contractItems)
                {
                    foreach(var contractItem in items)
                    {
                        var itemLookUp = await _unitOfWork.GetRepository<ContractItem>().GetFirstOrDefaultAsync(predicate: x => x.Id == contractItem.Id);
                        if(itemLookUp != null)
                        {
                            if(itemLookUp.Timeline.Year == contractItem.Timeline.Year)
                            {
                                itemLookUp.Timeline = contractItem.Timeline;
                            }
                            else
                            {
                                return new BaseResponse() { Status = false, Message = ResponseMessage.ContractDatePersist };
                            }
                            itemLookUp.Weighting = contractItem.Weighting;
                            itemLookUp.ScoreAchieved = contractItem.ScoreAchieved;
                            itemLookUp.WeightedSore = (contractItem.ScoreAchieved * contractItem.Weighting)/100;
                            itemLookUp.Remark = contractItem.Remark;
                        }
                    }
                }
                    
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<ContractObjective> GetByObjectiveId(Guid objectiveId)
        {
            var data = await _unitOfWork.GetRepository<ContractObjective>().GetFirstOrDefaultAsync(predicate: x => x.Id == objectiveId, null, include: c => c.Include(i => i.Employee));
            return data;
        }

        public async Task<IEnumerable<ContractItem>> GetItemByObjectiveId(Guid objectiveId)
        {
            var data = await _unitOfWork.GetRepository<ContractItem>().GetAllAsync(predicate: x => x.ContractObjectiveId == objectiveId);
            foreach(var item in data)
            {
                if(item.Remark == null)
                {
                    item.Remark = "No Remark Yet";
                    item.Timeline = item.Timeline.Date;
                }
            }
            return data;
        }

        public async Task<IEnumerable<ContractObjective>> GetByLineManager(string lineManagerNo)
        {
            var data = await _unitOfWork.GetRepository<ContractObjective>().GetAllAsync(predicate: x => x.LineManager.ToLower() == lineManagerNo.ToLower(), null, "Employee");
            return data;
        }

        public async Task<IEnumerable<ContractObjective>> GetByEmployee(Guid employeeId)
        {
            var data = await _unitOfWork.GetRepository<ContractObjective>().GetAllAsync(predicate: x => x.EmployeeId == employeeId, null, "Employee");
            return data;
        }
    }
}
