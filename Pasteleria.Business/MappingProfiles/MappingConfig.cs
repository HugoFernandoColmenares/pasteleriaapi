using AutoMapper;
using Pasteleria.Shared.Auth.Dtos;
using Pasteleria.Shared.Models;
using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Models;

namespace Pasteleria.Business.MappingProfiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // Users
            CreateMap<UserRegistrationRequestDto, User>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

            // Documents
            CreateMap<DocumentDto, Document>().ReverseMap();
            CreateMap<CreateDocumentDto, Document>().ReverseMap();
            CreateMap<ListDocumentDto, Document>().ReverseMap();

            // Ingredients
            CreateMap<IngredientDto, Ingredient>().ReverseMap();
            CreateMap<CreateIngredientDto, Ingredient>().ReverseMap();
            CreateMap<ListIngredientDto, Ingredient>().ReverseMap();
            CreateMap<UpdateIngredientDto, Ingredient>().ReverseMap();

            // Inventory
            CreateMap<InventoryItemDto, InventoryItem>().ReverseMap();
            CreateMap<CreateInventoryItemDto, InventoryItem>().ReverseMap();
            CreateMap<ListInventoryItemDto, InventoryItem>()
                .ForMember(dest => dest.Ingredient, opt => opt.Ignore()) // Ensure no entity leak
                .ReverseMap()
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient != null ? src.Ingredient.Name : string.Empty));

            // Recipe
            CreateMap<RecipeDto, Recipe>().ReverseMap();
            CreateMap<CreateRecipeDto, Recipe>().ReverseMap();
            CreateMap<ListRecipeDto, Recipe>().ReverseMap();

            // RecipeInventory (Ingredients in recipes)
            CreateMap<RecipeIngredientDto, RecipeIngredient>().ReverseMap();
            CreateMap<CreateRecipeIngredientDto, RecipeIngredient>();
            CreateMap<ListRecipeIngredientDto, RecipeIngredient>()
                .ReverseMap()
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient != null ? src.Ingredient.Name : string.Empty));
        }
    }
}
