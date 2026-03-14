using Pasteleria.Shared.Models;

namespace Pasteleria.Shared.DTOs
{
    public class RecipeIngredientDto
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public Guid IngredientId { get; set; }
        public IngredientDto? Ingredient { get; set; }

        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
    }

    public class CreateRecipeIngredientDto
    {
        public Guid IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
    }

    public class ListRecipeIngredientDto
    {
        public Guid Id { get; set; }
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}
