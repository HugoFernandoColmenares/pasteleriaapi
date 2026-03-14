using Microsoft.AspNetCore.Mvc;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;
using Pasteleria.Shared.Extensions;
using System.Net;

namespace Pasteleria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        /// <summary>
        /// Retrieves all uploaded documents.
        /// </summary>
        /// <returns>A list of documents.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ListDocumentDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _documentService.GetAllDocumentsAsync(pageNumber, pageSize);
            if (result.IsSuccessful && result.Data != null)
            {
                var pagination = MapToPaginationDto(result.Data);
                return Ok(ApiResponse<List<ListDocumentDto>>.SuccessResponse(result.Data.Items, "Documents retrieved successfully.", (int)HttpStatusCode.OK, pagination));
            }
            return BadRequest(ApiResponse<List<ListDocumentDto>>.FailureResponse("Failed to retrieve documents.", result.Errors));
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
        /// Retrieves a document by its ID.
        /// </summary>
        /// <param name="id">The document ID.</param>
        /// <returns>The document details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<DocumentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<DocumentDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _documentService.GetDocumentByIdAsync(id);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<DocumentDto>.SuccessResponse(result.Data, "Document retrieved successfully."));
            }
            return NotFound(ApiResponse<DocumentDto>.FailureResponse($"Document with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }

        /// <summary>
        /// Uploads a new document.
        /// </summary>
        /// <param name="documentDto">The document creation data.</param>
        /// <returns>The created document.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<DocumentDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<DocumentDto>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] CreateDocumentDto documentDto)
        {
            var result = await _documentService.AddDocumentAsync(documentDto);
            if (result.IsSuccessful)
            {
                var response = ApiResponse<DocumentDto>.SuccessResponse(result.Data, "Document uploaded successfully.", (int)HttpStatusCode.Created);
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, response);
            }
            return BadRequest(ApiResponse<DocumentDto>.FailureResponse("Failed to upload document.", result.Errors));
        }

        /// <summary>
        /// Updates document metadata.
        /// </summary>
        /// <param name="id">The ID of the document to update.</param>
        /// <param name="documentDto">The updated document data.</param>
        /// <returns>The updated document.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<DocumentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<DocumentDto>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<DocumentDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] DocumentDto documentDto)
        {
            if (id != documentDto.Id)
            {
                return BadRequest(ApiResponse<DocumentDto>.FailureResponse("Document ID in URL does not match ID in body."));
            }

            var result = await _documentService.UpdateDocumentAsync(documentDto);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<DocumentDto>.SuccessResponse(result.Data, "Document updated successfully."));
            }
            return BadRequest(ApiResponse<DocumentDto>.FailureResponse("Failed to update document.", result.Errors));
        }

        /// <summary>
        /// Deletes a document.
        /// </summary>
        /// <param name="id">The ID of the document to delete.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _documentService.DeleteDocumentAsync(id);
            if (result.IsSuccessful)
            {
                return NoContent();
            }
            return NotFound(ApiResponse<bool>.FailureResponse($"Document with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }
    }
}
