using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Interfaces.Repositories
{
    public interface IRecipeRepository
    {
        Task<List<Recipe>> GetAllAsync();
        Task<(List<Recipe> Items, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize);
        Task<Recipe?> GetByIdAsync(Guid id);
        Task AddAsync(Recipe entity);
        Task UpdateAsync(Recipe entity);
        Task DeleteAsync(Guid id);
    }
}
