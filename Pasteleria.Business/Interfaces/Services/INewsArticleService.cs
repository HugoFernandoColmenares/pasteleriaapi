using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;
using System;
using System.Threading.Tasks;

namespace Pasteleria.Business.Interfaces.Services
{
    public interface INewsArticleService
    {
        Task<Result<PagedList<ListNewsArticleDto>>> GetAllNewsArticlesAsync(int pageNumber, int pageSize);
        Task<Result<NewsArticleDto>> GetNewsArticleByIdAsync(Guid id);
        Task<Result<NewsArticleDto>> AddNewsArticleAsync(CreateNewsArticleDto newsArticleDto);
        Task<Result<NewsArticleDto>> UpdateNewsArticleAsync(UpdateNewsArticleDto newsArticleDto);
        Task<Result<bool>> DeleteNewsArticleAsync(Guid id);
    }
}
