using Microsoft.EntityFrameworkCore;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Data;
using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Repositories
{
    public class StorageLocationRepository : IStorageLocationRepository
    {
        private readonly Context _context;

        public StorageLocationRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<StorageLocation>> GetAllAsync()
        {
            return await _context.StorageLocations.AsNoTracking().ToListAsync();
        }

        public async Task<(List<StorageLocation> Items, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.StorageLocations.CountAsync();
            var items = await _context.StorageLocations
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<StorageLocation?> GetByIdAsync(Guid id)
        {
            return await _context.StorageLocations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(StorageLocation entity)
        {
            await _context.StorageLocations.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StorageLocation entity)
        {
            var existing = await _context.StorageLocations.FindAsync(entity.Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.StorageLocations.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
                _context.StorageLocations.Update(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
