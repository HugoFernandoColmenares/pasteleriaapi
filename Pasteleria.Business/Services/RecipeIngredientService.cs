using AutoMapper;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;
using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Services
{
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly IMapper _mapper;

        public RecipeIngredientService(IRecipeIngredientRepository recipeIngredientRepository, IMapper mapper)
        {
            _recipeIngredientRepository = recipeIngredientRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<ListRecipeIngredientDto>>> GetAllRecipeIngredientsAsync()
        {
            var recipeIngredients = await _recipeIngredientRepository.GetAllAsync();
            var recipeIngredientDtos = _mapper.Map<List<ListRecipeIngredientDto>>(recipeIngredients);
            return Result<List<ListRecipeIngredientDto>>.Success(recipeIngredientDtos);
        }

        public async Task<Result<RecipeIngredientDto>> GetRecipeIngredientByIdAsync(Guid id)
        {
            var recipeIngredient = await _recipeIngredientRepository.GetByIdAsync(id);
            if (recipeIngredient == null)
            {
                return Result<RecipeIngredientDto>.Failure(new List<string> { $"RecipeIngredient with ID {id} not found." });
            }
            var recipeIngredientDto = _mapper.Map<RecipeIngredientDto>(recipeIngredient);
            return Result<RecipeIngredientDto>.Success(recipeIngredientDto);
        }

        public async Task<Result<RecipeIngredientDto>> AddRecipeIngredientAsync(CreateRecipeIngredientDto recipeIngredientDto)
        {
            var recipeIngredient = _mapper.Map<RecipeIngredient>(recipeIngredientDto);
            await _recipeIngredientRepository.AddAsync(recipeIngredient);
            var createdRecipeIngredientDto = _mapper.Map<RecipeIngredientDto>(recipeIngredient);
            return Result<RecipeIngredientDto>.Success(createdRecipeIngredientDto);
        }

        public async Task<Result<RecipeIngredientDto>> UpdateRecipeIngredientAsync(RecipeIngredientDto recipeIngredientDto)
        {
            var existingRecipeIngredient = await _recipeIngredientRepository.GetByIdAsync(recipeIngredientDto.Id);
            if (existingRecipeIngredient == null)
            {
                return Result<RecipeIngredientDto>.Failure(new List<string> { $"RecipeIngredient with ID {recipeIngredientDto.Id} not found." });
            }

            _mapper.Map(recipeIngredientDto, existingRecipeIngredient);
            await _recipeIngredientRepository.UpdateAsync(existingRecipeIngredient);
            return Result<RecipeIngredientDto>.Success(recipeIngredientDto);
        }

        public async Task<Result<bool>> DeleteRecipeIngredientAsync(Guid id)
        {
            var existingRecipeIngredient = await _recipeIngredientRepository.GetByIdAsync(id);
            if (existingRecipeIngredient == null)
            {
                return Result<bool>.Failure(new List<string> { $"RecipeIngredient with ID {id} not found." });
            }

            await _recipeIngredientRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
    }
}
