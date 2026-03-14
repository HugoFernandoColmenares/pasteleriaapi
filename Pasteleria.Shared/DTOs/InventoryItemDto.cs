using Pasteleria.Shared.Models;

namespace Pasteleria.Shared.DTOs
{
    public class InventoryItemDto
    {
        public Guid Id { get; set; }
        public Guid IngredientId { get; set; }
        public IngredientDto? Ingredient { get; set; }
        public decimal CurrentQuantity { get; set; }
        public decimal MinimumQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool IsLowStock => CurrentQuantity < MinimumQuantity;
    }

    public class CreateInventoryItemDto
    {
        public Guid IngredientId { get; set; }
        public decimal CurrentQuantity { get; set; }
        public decimal MinimumQuantity { get; set; }
        public string Location { get; set; } = string.Empty;
    }

    public class ListInventoryItemDto
    {
        public Guid Id { get; set; }
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public decimal CurrentQuantity { get; set; }
        public decimal MinimumQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}
