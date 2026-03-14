namespace Pasteleria.Shared.Models
{
    public class Recipe : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public decimal TotalCost { get; private set; }
        public decimal SuggestedPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

        // Método para calcular el costo total
        public void CalculateTotalCost()
        {
            TotalCost = RecipeIngredients.Sum(ri => ri.Ingredient.CostPerUnit * ri.Quantity);
        }
    }
}
