using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;

namespace Pasteleria.Business.Interfaces.Services
{
    public interface IRecipeIngredientService
    {
        Task<Result<List<ListRecipeIngredientDto>>> GetAllRecipeIngredientsAsync();
        Task<Result<RecipeIngredientDto>> GetRecipeIngredientByIdAsync(Guid id);
        Task<Result<RecipeIngredientDto>> AddRecipeIngredientAsync(CreateRecipeIngredientDto recipeIngredientDto);
        Task<Result<RecipeIngredientDto>> UpdateRecipeIngredientAsync(RecipeIngredientDto recipeIngredientDto);
        Task<Result<bool>> DeleteRecipeIngredientAsync(Guid id);
    }
}
