namespace Pasteleria.Shared.DTOs
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public string UploadedByUserId { get; set; } = string.Empty;
    }

    public class CreateDocumentDto
    {
        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        // UploadedByUserId removed as it should be taken from Auth context
    }

    public class ListDocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public string UploadedByUserId { get; set; } = string.Empty;
    }
}
