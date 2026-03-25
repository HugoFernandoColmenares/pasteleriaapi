using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Interfaces.Repositories
{
    public interface IStorageLocationRepository
    {
        Task<List<StorageLocation>> GetAllAsync();
        Task<(List<StorageLocation> Items, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize);
        Task<StorageLocation?> GetByIdAsync(Guid id);
        Task AddAsync(StorageLocation entity);
        Task UpdateAsync(StorageLocation entity);
        Task DeleteAsync(Guid id);
    }
}
