using Pasteleria.Data;
using Microsoft.EntityFrameworkCore;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly Context _context;

        public IngredientRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(Ingredient dto)
        {
            await _context.Ingredients.AddAsync(dto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await _context.Ingredients.IgnoreQueryFilters().FirstOrDefaultAsync(i => i.Id == id);
            if (result != null)
            {
                result.IsDeleted = true;
                result.DeletedAt = DateTime.UtcNow;
                _context.Ingredients.Update(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Ingredient>> GetAllAsync()
        {
            return await _context.Ingredients.AsNoTracking().Include(ri => ri.RecipeIngredients).Include(i => i.InventoryItems).ToListAsync();
        }

        public async Task<(List<Ingredient> Items, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Ingredients.CountAsync();
            var items = await _context.Ingredients
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Ingredient?> GetByIdAsync(Guid id)
        {
            return await _context.Ingredients.Include(ri=> ri.RecipeIngredients).Include(i => i.InventoryItems).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Ingredient dto)
        {
            _context.Ingredients.Update(dto);
            await _context.SaveChangesAsync();
        }
    }
}
