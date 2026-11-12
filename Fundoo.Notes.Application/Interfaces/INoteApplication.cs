using Fundoo.Notes.Domain.Entities;

namespace Fundoo.Notes.Application.Interfaces
{
    public interface INoteApplication
    {
        Task<IEnumerable<Note>> GetUserNotesAsync(string userId, CancellationToken ct);
        Task<Note> CreateNoteAsync(string userId, string title, string description, string color, CancellationToken ct);
        Task<Note?> UpdateNoteAsync(Guid id, string userId, string title, string description, string color, bool isPinned, bool isArchived, CancellationToken ct);
        Task<bool> DeleteNoteAsync(Guid id, string userId, CancellationToken ct);
    }
}