using AutoMapper;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;
using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;

        public InventoryItemService(IInventoryItemRepository inventoryItemRepository, IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<ListInventoryItemDto>>> GetAllInventoryItemsAsync(int pageNumber, int pageSize)
        {
            var (inventoryItems, totalCount) = await _inventoryItemRepository.GetAllPaginatedAsync(pageNumber, pageSize);
            var inventoryItemDtos = _mapper.Map<List<ListInventoryItemDto>>(inventoryItems);

            var pagedList = new PagedList<ListInventoryItemDto>(inventoryItemDtos, totalCount, pageNumber, pageSize);
            return Result<PagedList<ListInventoryItemDto>>.Success(pagedList);
        }

        public async Task<Result<InventoryItemDto>> GetInventoryItemByIdAsync(Guid id)
        {
            var inventoryItem = await _inventoryItemRepository.GetByIdAsync(id);
            if (inventoryItem == null)
            {
                return Result<InventoryItemDto>.Failure(new List<string> { $"InventoryItem with ID {id} not found." });
            }
            var inventoryItemDto = _mapper.Map<InventoryItemDto>(inventoryItem);
            return Result<InventoryItemDto>.Success(inventoryItemDto);
        }

        public async Task<Result<InventoryItemDto>> AddInventoryItemAsync(CreateInventoryItemDto inventoryItemDto)
        {
            try
            {
                // Validate ingredient existence
                var ingredient = await _ingredientRepository.GetByIdAsync(inventoryItemDto.IngredientId);
                if (ingredient == null)
                {
                    return Result<InventoryItemDto>.Failure(new List<string> { $"Ingredient with ID {inventoryItemDto.IngredientId} not found." });
                }

                var inventoryItem = _mapper.Map<InventoryItem>(inventoryItemDto);
                inventoryItem.Id = Guid.NewGuid();
                inventoryItem.LastUpdated = DateTime.UtcNow;

                await _inventoryItemRepository.AddAsync(inventoryItem);
                
                var createdInventoryItemDto = _mapper.Map<InventoryItemDto>(inventoryItem);
                return Result<InventoryItemDto>.Success(createdInventoryItemDto);
            }
            catch (Exception ex)
            {
                return Result<InventoryItemDto>.Failure(new List<string> { "An error occurred while adding the inventory item.", ex.Message });
            }
        }

        public async Task<Result<InventoryItemDto>> UpdateInventoryItemAsync(InventoryItemDto inventoryItemDto)
        {
            try
            {
                var existingInventoryItem = await _inventoryItemRepository.GetByIdAsync(inventoryItemDto.Id);
                if (existingInventoryItem == null)
                {
                    return Result<InventoryItemDto>.Failure(new List<string> { $"InventoryItem with ID {inventoryItemDto.Id} not found." });
                }

                // Validate ingredient existence if it's being changed
                var ingredient = await _ingredientRepository.GetByIdAsync(inventoryItemDto.IngredientId);
                if (ingredient == null)
                {
                    return Result<InventoryItemDto>.Failure(new List<string> { $"Ingredient with ID {inventoryItemDto.IngredientId} not found." });
                }

                _mapper.Map(inventoryItemDto, existingInventoryItem);
                existingInventoryItem.LastUpdated = DateTime.UtcNow;

                await _inventoryItemRepository.UpdateAsync(existingInventoryItem);
                return Result<InventoryItemDto>.Success(inventoryItemDto);
            }
            catch (Exception ex)
            {
                return Result<InventoryItemDto>.Failure(new List<string> { "An error occurred while updating the inventory item.", ex.Message });
            }
        }

        public async Task<Result<bool>> DeleteInventoryItemAsync(Guid id)
        {
            var existingInventoryItem = await _inventoryItemRepository.GetByIdAsync(id);
            if (existingInventoryItem == null)
            {
                return Result<bool>.Failure(new List<string> { $"InventoryItem with ID {id} not found." });
            }

            await _inventoryItemRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
    }
}
