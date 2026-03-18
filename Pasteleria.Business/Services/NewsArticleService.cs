using AutoMapper;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;
using Pasteleria.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pasteleria.Business.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly IMapper _mapper;

        public NewsArticleService(INewsArticleRepository newsArticleRepository, IMapper mapper)
        {
            _newsArticleRepository = newsArticleRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<ListNewsArticleDto>>> GetAllNewsArticlesAsync(int pageNumber, int pageSize)
        {
            var (articles, totalCount) = await _newsArticleRepository.GetAllPaginatedAsync(pageNumber, pageSize);
            var articleDtos = _mapper.Map<List<ListNewsArticleDto>>(articles);
            
            var pagedList = new PagedList<ListNewsArticleDto>(articleDtos, totalCount, pageNumber, pageSize);
            return Result<PagedList<ListNewsArticleDto>>.Success(pagedList);
        }

        public async Task<Result<NewsArticleDto>> GetNewsArticleByIdAsync(Guid id)
        {
            var article = await _newsArticleRepository.GetByIdAsync(id);
            if (article == null)
            {
                return Result<NewsArticleDto>.Failure(new List<string> { $"News article with ID {id} not found." });
            }
            var articleDto = _mapper.Map<NewsArticleDto>(article);
            return Result<NewsArticleDto>.Success(articleDto);
        }

        public async Task<Result<NewsArticleDto>> AddNewsArticleAsync(CreateNewsArticleDto newsArticleDto)
        {
            var article = _mapper.Map<NewsArticle>(newsArticleDto);
            article.Id = Guid.NewGuid();
            if (article.PublishedAt == default)
            {
                article.PublishedAt = DateTime.UtcNow;
            }
            await _newsArticleRepository.AddAsync(article);
            var createdArticleDto = _mapper.Map<NewsArticleDto>(article);
            return Result<NewsArticleDto>.Success(createdArticleDto);
        }

        public async Task<Result<NewsArticleDto>> UpdateNewsArticleAsync(UpdateNewsArticleDto newsArticleDto)
        {
            var existingArticle = await _newsArticleRepository.GetByIdAsync(newsArticleDto.Id);
            if (existingArticle == null)
            {
                return Result<NewsArticleDto>.Failure(new List<string> { $"News article with ID {newsArticleDto.Id} not found." });
            }

            _mapper.Map(newsArticleDto, existingArticle);
            existingArticle.UpdatedAt = DateTime.UtcNow;
            await _newsArticleRepository.UpdateAsync(existingArticle);
            
            var updatedDto = _mapper.Map<NewsArticleDto>(existingArticle);
            return Result<NewsArticleDto>.Success(updatedDto);
        }

        public async Task<Result<bool>> DeleteNewsArticleAsync(Guid id)
        {
            var existingArticle = await _newsArticleRepository.GetByIdAsync(id);
            if (existingArticle == null)
            {
                return Result<bool>.Failure(new List<string> { $"News article with ID {id} not found." });
            }

            await _newsArticleRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
    }
}
