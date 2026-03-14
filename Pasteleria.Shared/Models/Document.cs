namespace Pasteleria.Shared.Models
{
    public class Document : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public string UploadedByUserId { get; set; } = string.Empty;
    }
}
