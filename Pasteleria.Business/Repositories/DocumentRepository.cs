using Pasteleria.Data;
using Microsoft.EntityFrameworkCore;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly Context _context;

        public DocumentRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(Document dto)
        {
            await _context.Documents.AddAsync(dto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await _context.Documents.IgnoreQueryFilters().FirstOrDefaultAsync(d => d.Id == id);
            if (result != null)
            {
                result.IsDeleted = true;
                result.DeletedAt = DateTime.UtcNow;
                _context.Documents.Update(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Document>> GetAllAsync()
        {
            return await _context.Documents.AsNoTracking().ToListAsync();
        }

        public async Task<(List<Document> Items, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Documents.CountAsync();
            var items = await _context.Documents
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Document?> GetByIdAsync(Guid id)
        {
            return await _context.Documents.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Document dto)
        {
            _context.Documents.Update(dto);
            await _context.SaveChangesAsync();
        }
    }
}
