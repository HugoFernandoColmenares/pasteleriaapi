using Pasteleria.Shared.Models;

namespace Pasteleria.Shared.DTOs
{
    public class RecipeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }
        public decimal SuggestedPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        // DTOs instead of Entities to avoid redundancy and circular references
        public List<RecipeIngredientDto> RecipeIngredients { get; set; } = new List<RecipeIngredientDto>();
    }

    public class CreateRecipeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public decimal SuggestedPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        // Only IDs and quantities needed for creation
        public List<CreateRecipeIngredientDto> RecipeIngredients { get; set; } = new List<CreateRecipeIngredientDto>();
    }

    public class ListRecipeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }
        public decimal SuggestedPrice { get; set; }
        // No nested data here to keep lists light
    }
}
