using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Interfaces.Repositories
{
    public interface IRecipeIngredientRepository
    {
        Task<List<RecipeIngredient>> GetAllAsync();
        Task<RecipeIngredient?> GetByIdAsync(Guid id);
        Task AddAsync(RecipeIngredient entity);
        Task UpdateAsync(RecipeIngredient entity);
        Task DeleteAsync(Guid id);
    }
}
