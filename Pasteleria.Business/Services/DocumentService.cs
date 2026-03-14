using AutoMapper;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;
using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;

        public DocumentService(IDocumentRepository documentRepository, IMapper mapper)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<ListDocumentDto>>> GetAllDocumentsAsync(int pageNumber, int pageSize)
        {
            var (documents, totalCount) = await _documentRepository.GetAllPaginatedAsync(pageNumber, pageSize);
            var documentDtos = _mapper.Map<List<ListDocumentDto>>(documents);

            var pagedList = new PagedList<ListDocumentDto>(documentDtos, totalCount, pageNumber, pageSize);
            return Result<PagedList<ListDocumentDto>>.Success(pagedList);
        }

        public async Task<Result<DocumentDto>> GetDocumentByIdAsync(Guid id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null)
            {
                return Result<DocumentDto>.Failure(new List<string> { $"Document with ID {id} not found." });
            }
            var documentDto = _mapper.Map<DocumentDto>(document);
            return Result<DocumentDto>.Success(documentDto);
        }

        public async Task<Result<DocumentDto>> AddDocumentAsync(CreateDocumentDto documentDto)
        {
            var document = _mapper.Map<Document>(documentDto);
            await _documentRepository.AddAsync(document);
            var createdDocumentDto = _mapper.Map<DocumentDto>(document);
            return Result<DocumentDto>.Success(createdDocumentDto);
        }

        public async Task<Result<DocumentDto>> UpdateDocumentAsync(DocumentDto documentDto)
        {
            var existingDocument = await _documentRepository.GetByIdAsync(documentDto.Id);
            if (existingDocument == null)
            {
                return Result<DocumentDto>.Failure(new List<string> { $"Document with ID {documentDto.Id} not found." });
            }

            _mapper.Map(documentDto, existingDocument);
            await _documentRepository.UpdateAsync(existingDocument);
            return Result<DocumentDto>.Success(documentDto);
        }

        public async Task<Result<bool>> DeleteDocumentAsync(Guid id)
        {
            var existingDocument = await _documentRepository.GetByIdAsync(id);
            if (existingDocument == null)
            {
                return Result<bool>.Failure(new List<string> { $"Document with ID {id} not found." });
            }

            await _documentRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
    }
}
