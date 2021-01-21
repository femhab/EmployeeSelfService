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
        private readonly INotificationService _notificationService;
        
        public ContractService(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<BaseResponse> Create(ContractObjective model, List<ContractItem> items)
        {
            if(model != null && items.Count() > 0)
            {
                try
                {
                    var check = await _unitOfWork.GetRepository<ContractObjective>().GetFirstOrDefaultAsync(predicate: x => x.EmployeeId == model.EmployeeId && x.CreatedDate.Year == DateTime.Now.Year);
                    if(check == null)
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
                        await _notificationService.CreateNotification(NotificationAction.ObjectiveCreateTitle, NotificationAction.ObjectiveCreateMessage, model.EmployeeId, false, false);

                        //send text to the line manager

                        return new BaseResponse() { Status = true, Message = ResponseMessage.ContractCreated };
                    }
                    return new BaseResponse() { Status = false, Message = ResponseMessage.ContractExist };
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

                        //send email to employee
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

        public async Task<BaseResponse> SignOffContract(Guid contractId)
        {
            var data = await _unitOfWork.GetRepository<ContractObjective>().GetFirstOrDefaultAsync(predicate: x => x.Id == contractId);
            if(data != null)
            {
                data.IsSignedOff = true;
                data.SignedOffDate = DateTime.Now;
                _unitOfWork.GetRepository<ContractObjective>().Update(data);
                await _unitOfWork.SaveChangesAsync();
                //update commnet
            }
            return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
        }

        public async Task<BaseResponse> LMSignOffContract(Guid contractId)
        {
            var data = await _unitOfWork.GetRepository<ContractObjective>().GetFirstOrDefaultAsync(predicate: x => x.Id == contractId);
            if (data != null)
            {
                data.IsHRSignedOff = true;
                data.SignedOffDate = DateTime.Now;
                _unitOfWork.GetRepository<ContractObjective>().Update(data);
                await _unitOfWork.SaveChangesAsync();
            }
            return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
        }


        public async Task<ContractObjective> GetById(Guid id)
        {
            var data = await _unitOfWork.GetRepository<ContractObjective>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            return data;
        }

        public async Task<ContractItem> GetContractItemById(Guid itemId)
        {
            var data = await _unitOfWork.GetRepository<ContractItem>().GetFirstOrDefaultAsync(predicate: x => x.Id == itemId);
            return data;
        }

        public async Task<BaseResponse> UpdateContractItem(List<ContractItem> model)
        {
            _unitOfWork.GetRepository<ContractItem>().Update(model);
            await _unitOfWork.SaveChangesAsync();

            return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
        }

        public async Task<BaseResponse> UpdateContractObjective(ContractObjective model)
        {
            _unitOfWork.GetRepository<ContractObjective>().Update(model);
            await _unitOfWork.SaveChangesAsync();

            return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
        }
    }
}
