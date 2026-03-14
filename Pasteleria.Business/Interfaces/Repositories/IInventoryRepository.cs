using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Interfaces.Repositories
{
    public interface IInventoryRepository
    {
        Task<InventoryItem> GetByIdAsync(Guid id);
        Task<List<InventoryItem>> GetAllAsync();
        Task AddAsync(InventoryItem dto);
        Task UpdateAsync(InventoryItem dto);
        Task DeleteAsync(Guid id);
    }
}
