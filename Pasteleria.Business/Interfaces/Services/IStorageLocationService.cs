using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;

namespace Pasteleria.Business.Interfaces.Services
{
    public interface IStorageLocationService
    {
        Task<Result<IEnumerable<StorageLocationDto>>> GetAllStorageLocationsAsync();
        Task<Result<StorageLocationDto>> GetStorageLocationByIdAsync(Guid id);
        Task<Result<StorageLocationDto>> CreateStorageLocationAsync(CreateStorageLocationDto storageLocationDto);
        Task<Result<StorageLocationDto>> UpdateStorageLocationAsync(StorageLocationDto storageLocationDto);
        Task<Result<bool>> DeleteStorageLocationAsync(Guid id);
    }
}
