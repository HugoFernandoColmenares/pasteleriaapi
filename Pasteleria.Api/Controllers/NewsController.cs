using Microsoft.AspNetCore.Mvc;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Pasteleria.Api.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        /// <summary>
        /// Retrieves all news articles.
        /// </summary>
        /// <returns>A list of news articles.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ListNewsArticleDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _newsArticleService.GetAllNewsArticlesAsync(pageNumber, pageSize);
            if (result.IsSuccessful && result.Data != null)
            {
                var pagination = MapToPaginationDto(result.Data);
                return Ok(ApiResponse<List<ListNewsArticleDto>>.SuccessResponse(result.Data.Items, "News articles retrieved successfully.", (int)HttpStatusCode.OK, pagination));
            }
            return BadRequest(ApiResponse<List<ListNewsArticleDto>>.FailureResponse("Failed to retrieve news articles.", result.Errors));
        }

        private PaginationDto MapToPaginationDto<T>(PagedList<T> pagedList)
        {
            return new PaginationDto
            {
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                PageSize = pagedList.PageSize,
                TotalCount = pagedList.TotalCount,
                HasPrevious = pagedList.HasPrevious,
                HasNext = pagedList.HasNext
            };
        }

        /// <summary>
        /// Retrieves a news article by its ID.
        /// </summary>
        /// <param name="id">The news article ID.</param>
        /// <returns>The news article details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<NewsArticleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<NewsArticleDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _newsArticleService.GetNewsArticleByIdAsync(id);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<NewsArticleDto>.SuccessResponse(result.Data, "News article retrieved successfully."));
            }
            return NotFound(ApiResponse<NewsArticleDto>.FailureResponse($"News article with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }

        /// <summary>
        /// Creates a new news article.
        /// </summary>
        /// <param name="newsArticleDto">The news article creation data.</param>
        /// <returns>The created news article.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<NewsArticleDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<NewsArticleDto>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] CreateNewsArticleDto newsArticleDto)
        {
            var result = await _newsArticleService.AddNewsArticleAsync(newsArticleDto);
            if (result.IsSuccessful)
            {
                var response = ApiResponse<NewsArticleDto>.SuccessResponse(result.Data, "News article created successfully.", (int)HttpStatusCode.Created);
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, response);
            }
            return BadRequest(ApiResponse<NewsArticleDto>.FailureResponse("Failed to create news article.", result.Errors));
        }

        /// <summary>
        /// Updates an existing news article.
        /// </summary>
        /// <param name="id">The ID of the news article to update.</param>
        /// <param name="newsArticleDto">The updated news article data.</param>
        /// <returns>The updated news article.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<NewsArticleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<NewsArticleDto>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<NewsArticleDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNewsArticleDto newsArticleDto)
        {
            if (id != newsArticleDto.Id)
            {
                return BadRequest(ApiResponse<NewsArticleDto>.FailureResponse("News article ID in URL does not match News article ID in body."));
            }

            var result = await _newsArticleService.UpdateNewsArticleAsync(newsArticleDto);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<NewsArticleDto>.SuccessResponse(result.Data, "News article updated successfully."));
            }
            return BadRequest(ApiResponse<NewsArticleDto>.FailureResponse("Failed to update news article.", result.Errors));
        }

        /// <summary>
        /// Deletes a news article by its ID.
        /// </summary>
        /// <param name="id">The ID of the news article to delete.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _newsArticleService.DeleteNewsArticleAsync(id);
            if (result.IsSuccessful)
            {
                return NoContent();
            }
            return NotFound(ApiResponse<bool>.FailureResponse($"News article with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }
    }
}
