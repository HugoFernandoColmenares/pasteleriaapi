using AutoMapper;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;
using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;

        public IngredientService(IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<ListIngredientDto>>> GetAllIngredientsAsync(int pageNumber, int pageSize)
        {
            var (ingredients, totalCount) = await _ingredientRepository.GetAllPaginatedAsync(pageNumber, pageSize);
            var ingredientDtos = _mapper.Map<List<ListIngredientDto>>(ingredients);
            
            var pagedList = new PagedList<ListIngredientDto>(ingredientDtos, totalCount, pageNumber, pageSize);
            return Result<PagedList<ListIngredientDto>>.Success(pagedList);
        }

        public async Task<Result<IngredientDto>> GetIngredientByIdAsync(Guid id)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id);
            if (ingredient == null)
            {
                return Result<IngredientDto>.Failure(new List<string> { $"Ingredient with ID {id} not found." });
            }
            var ingredientDto = _mapper.Map<IngredientDto>(ingredient);
            return Result<IngredientDto>.Success(ingredientDto);
        }

        public async Task<Result<IngredientDto>> AddIngredientAsync(CreateIngredientDto ingredientDto)
        {
            var ingredient = _mapper.Map<Ingredient>(ingredientDto);
            ingredient.Id = Guid.NewGuid();
            await _ingredientRepository.AddAsync(ingredient);
            var createdIngredientDto = _mapper.Map<IngredientDto>(ingredient);
            return Result<IngredientDto>.Success(createdIngredientDto);
        }

        public async Task<Result<IngredientDto>> UpdateIngredientAsync(IngredientDto ingredientDto)
        {
            var existingIngredient = await _ingredientRepository.GetByIdAsync(ingredientDto.Id);
            if (existingIngredient == null)
            {
                return Result<IngredientDto>.Failure(new List<string> { $"Ingredient with ID {ingredientDto.Id} not found." });
            }

            _mapper.Map(ingredientDto, existingIngredient);
            await _ingredientRepository.UpdateAsync(existingIngredient);
            return Result<IngredientDto>.Success(ingredientDto);
        }

        public async Task<Result<bool>> DeleteIngredientAsync(Guid id)
        {
            var existingIngredient = await _ingredientRepository.GetByIdAsync(id);
            if (existingIngredient == null)
            {
                return Result<bool>.Failure(new List<string> { $"Ingredient with ID {id} not found." });
            }

            await _ingredientRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
    }
}
