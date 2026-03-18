using Pasteleria.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pasteleria.Business.Interfaces.Repositories
{
    public interface INewsArticleRepository
    {
        Task<List<NewsArticle>> GetAllAsync();
        Task<(List<NewsArticle> Items, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize);
        Task<NewsArticle?> GetByIdAsync(Guid id);
        Task AddAsync(NewsArticle entity);
        Task UpdateAsync(NewsArticle entity);
        Task DeleteAsync(Guid id);
    }
}
