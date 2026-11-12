namespace Fundoo.Notes.Domain.Entities
{
    public class Note
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = string.Empty; // JWT Token ton aayegi
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = "#FFFFFF";
        public bool IsPinned { get; set; } = false;
        public bool IsArchived { get; set; } = false;
        public bool IsTrash { get; set; } = false;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}