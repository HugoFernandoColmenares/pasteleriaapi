using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;

namespace Pasteleria.Business.Interfaces.Services
{
    public interface IIngredientService
    {
        Task<Result<PagedList<ListIngredientDto>>> GetAllIngredientsAsync(int pageNumber, int pageSize);
        Task<Result<IngredientDto>> GetIngredientByIdAsync(Guid id);
        Task<Result<IngredientDto>> AddIngredientAsync(CreateIngredientDto ingredientDto);
        Task<Result<IngredientDto>> UpdateIngredientAsync(IngredientDto ingredientDto);
        Task<Result<bool>> DeleteIngredientAsync(Guid id);
    }
}
