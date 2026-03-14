using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Interfaces.Repositories
{
    public interface IDocumentRepository
    {
        Task<List<Document>> GetAllAsync();
        Task<(List<Document> Items, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize);
        Task<Document?> GetByIdAsync(Guid id);
        Task AddAsync(Document entity);
        Task UpdateAsync(Document entity);
        Task DeleteAsync(Guid id);
    }
}
