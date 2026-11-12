namespace Fundoo.Notes.Api.Models
{
    public class CreateNoteRequestModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = "#FFFFFF";
    }
}