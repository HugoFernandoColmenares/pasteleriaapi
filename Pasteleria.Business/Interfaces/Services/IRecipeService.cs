using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;

namespace Pasteleria.Business.Interfaces.Services
{
    public interface IRecipeService
    {
        Task<Result<PagedList<ListRecipeDto>>> GetAllRecipesAsync(int pageNumber, int pageSize);
        Task<Result<RecipeDto>> GetRecipeByIdAsync(Guid id);
        Task<Result<RecipeDto>> AddRecipeAsync(CreateRecipeDto recipeDto);
        Task<Result<RecipeDto>> UpdateRecipeAsync(RecipeDto recipeDto);
        Task<Result<bool>> DeleteRecipeAsync(Guid id);
    }
}
