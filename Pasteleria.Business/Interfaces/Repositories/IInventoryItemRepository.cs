using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Interfaces.Repositories
{
    public interface IInventoryItemRepository
    {
        Task<List<InventoryItem>> GetAllAsync();
        Task<(List<InventoryItem> Items, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize);
        Task<InventoryItem?> GetByIdAsync(Guid id);
        Task AddAsync(InventoryItem entity);
        Task UpdateAsync(InventoryItem entity);
        Task DeleteAsync(Guid id);
    }
}
