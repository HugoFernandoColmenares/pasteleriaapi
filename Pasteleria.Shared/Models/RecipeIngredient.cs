namespace Pasteleria.Shared.Models
{
    public class RecipeIngredient : BaseEntity
    {
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; } = new Recipe();
        
        public Guid IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } = new Ingredient();

        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty; // "g", "kg", "ml", etc.
    }
}
