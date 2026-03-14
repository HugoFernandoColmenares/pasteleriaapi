using Pasteleria.Shared.Models;

namespace Pasteleria.Shared.DTOs
{
    public class IngredientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal CostPerUnit { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string SupplierInfo { get; set; } = string.Empty;
    }

    public class CreateIngredientDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal CostPerUnit { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string SupplierInfo { get; set; } = string.Empty;
    }

    public class ListIngredientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal CostPerUnit { get; set; }
        public string Unit { get; set; } = string.Empty;
    }

    public class UpdateIngredientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal CostPerUnit { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string SupplierInfo { get; set; } = string.Empty;
    }
}
