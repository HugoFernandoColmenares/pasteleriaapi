using Microsoft.EntityFrameworkCore;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Data;
using Pasteleria.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pasteleria.Business.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly Context _context;

        public NewsArticleRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(NewsArticle entity)
        {
            await _context.NewsArticles.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await _context.NewsArticles.IgnoreQueryFilters().FirstOrDefaultAsync(i => i.Id == id);
            if (result != null)
            {
                result.IsDeleted = true;
                result.DeletedAt = DateTime.UtcNow;
                _context.NewsArticles.Update(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<NewsArticle>> GetAllAsync()
        {
            return await _context.NewsArticles.AsNoTracking().ToListAsync();
        }

        public async Task<(List<NewsArticle> Items, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.NewsArticles.CountAsync();
            var items = await _context.NewsArticles
                .AsNoTracking()
                .OrderByDescending(n => n.PublishedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<NewsArticle?> GetByIdAsync(Guid id)
        {
            return await _context.NewsArticles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(NewsArticle entity)
        {
            _context.NewsArticles.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
