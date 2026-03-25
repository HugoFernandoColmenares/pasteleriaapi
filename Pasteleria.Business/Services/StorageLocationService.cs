using AutoMapper;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;
using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Services
{
    public class StorageLocationService : IStorageLocationService
    {
        private readonly IStorageLocationRepository _repository;
        private readonly IMapper _mapper;

        public StorageLocationService(IStorageLocationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<StorageLocationDto>>> GetAllStorageLocationsAsync()
        {
            var locations = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<StorageLocationDto>>(locations);
            return Result<IEnumerable<StorageLocationDto>>.Success(dtos);
        }

        public async Task<Result<StorageLocationDto>> GetStorageLocationByIdAsync(Guid id)
        {
            var location = await _repository.GetByIdAsync(id);
            if (location == null)
            {
                return Result<StorageLocationDto>.Failure(new List<string> { $"Storage location with ID {id} not found." });
            }
            var dto = _mapper.Map<StorageLocationDto>(location);
            return Result<StorageLocationDto>.Success(dto);
        }

        public async Task<Result<StorageLocationDto>> CreateStorageLocationAsync(CreateStorageLocationDto storageLocationDto)
        {
            var location = _mapper.Map<StorageLocation>(storageLocationDto);
            location.Id = Guid.NewGuid();
            await _repository.AddAsync(location);
            var resultDto = _mapper.Map<StorageLocationDto>(location);
            return Result<StorageLocationDto>.Success(resultDto);
        }

        public async Task<Result<StorageLocationDto>> UpdateStorageLocationAsync(StorageLocationDto storageLocationDto)
        {
            var existing = await _repository.GetByIdAsync(storageLocationDto.Id);
            if (existing == null)
            {
                return Result<StorageLocationDto>.Failure(new List<string> { $"Storage location with ID {storageLocationDto.Id} not found." });
            }

            _mapper.Map(storageLocationDto, existing);
            existing.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(existing);
            return Result<StorageLocationDto>.Success(storageLocationDto);
        }

        public async Task<Result<bool>> DeleteStorageLocationAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return Result<bool>.Failure(new List<string> { $"Storage location with ID {id} not found." });
            }

            await _repository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
    }
}
