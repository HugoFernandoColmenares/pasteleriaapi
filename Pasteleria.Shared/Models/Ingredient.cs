namespace Pasteleria.Shared.Models
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal CostPerUnit { get; set; }
        public string Unit { get; set; } = string.Empty; // Unidad base (gramos, litros, etc.)
        public double Presentation { get; set; } = 0; // Presentación de producto (500 gr, 1L, etc.).
        public string SupplierInfo { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    }
}
