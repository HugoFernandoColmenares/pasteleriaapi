using Pasteleria.Data;
using Microsoft.EntityFrameworkCore;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Repositories
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        private readonly Context _context;

        public InventoryItemRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(InventoryItem dto)
        {
            await _context.InventoryItems.AddAsync(dto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await _context.InventoryItems.IgnoreQueryFilters().FirstOrDefaultAsync(i => i.Id == id);
            if (result != null)
            {
                result.IsDeleted = true;
                result.DeletedAt = DateTime.UtcNow;
                _context.InventoryItems.Update(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<InventoryItem>> GetAllAsync()
        {
            return await _context.InventoryItems.AsNoTracking().ToListAsync();
        }

        public async Task<(List<InventoryItem> Items, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.InventoryItems.CountAsync();
            var items = await _context.InventoryItems
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<InventoryItem?> GetByIdAsync(Guid id)
        {
            return await _context.InventoryItems.Include(i => i.Ingredient).AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(InventoryItem dto)
        {
            _context.InventoryItems.Update(dto);
            await _context.SaveChangesAsync();
        }
    }
}
