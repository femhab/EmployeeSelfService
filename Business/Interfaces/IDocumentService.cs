using Data.Entities;
using Data.Enums;
using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IDocumentService
    {
        Task<BaseResponse> CreateOrUpdate(Document model);
        Task<Document> GetByReference(Guid referenceId, DocumentType type);
    }
}
