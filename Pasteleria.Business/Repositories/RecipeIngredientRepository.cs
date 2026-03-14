using Pasteleria.Data;
using Microsoft.EntityFrameworkCore;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Repositories
{
    public class RecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly Context _context;

        public RecipeIngredientRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(RecipeIngredient dto)
        {
            await _context.RecipeIngredients.AddAsync(dto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await _context.RecipeIngredients.IgnoreQueryFilters().FirstOrDefaultAsync(ri => ri.Id == id);
            if (result != null)
            {
                result.IsDeleted = true;
                result.DeletedAt = DateTime.UtcNow;
                _context.RecipeIngredients.Update(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<RecipeIngredient>> GetAllAsync()
        {
            return await _context.RecipeIngredients.AsNoTracking().ToListAsync();
        }

        public async Task<RecipeIngredient?> GetByIdAsync(Guid id)
        {
            return await _context.RecipeIngredients.FirstOrDefaultAsync(ri => ri.Id == id);
        }

        public async Task UpdateAsync(RecipeIngredient dto)
        {
            _context.RecipeIngredients.Update(dto);
            await _context.SaveChangesAsync();
        }
    }
}
