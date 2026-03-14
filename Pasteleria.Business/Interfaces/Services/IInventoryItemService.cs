using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;

namespace Pasteleria.Business.Interfaces.Services
{
    public interface IInventoryItemService
    {
        Task<Result<PagedList<ListInventoryItemDto>>> GetAllInventoryItemsAsync(int pageNumber, int pageSize);
        Task<Result<InventoryItemDto>> GetInventoryItemByIdAsync(Guid id);
        Task<Result<InventoryItemDto>> AddInventoryItemAsync(CreateInventoryItemDto inventoryItemDto);
        Task<Result<InventoryItemDto>> UpdateInventoryItemAsync(InventoryItemDto inventoryItemDto);
        Task<Result<bool>> DeleteInventoryItemAsync(Guid id);
    }
}
