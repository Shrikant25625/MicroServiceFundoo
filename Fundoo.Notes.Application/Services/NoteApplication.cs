using Fundoo.Notes.Application.Interfaces;
using Fundoo.Notes.Domain.Entities;

namespace Fundoo.Notes.Application.Services
{
    public class NoteApplication(INoteRepository noteRepository) : INoteApplication
    {
        public async Task<IEnumerable<Note>> GetUserNotesAsync(string userId, CancellationToken ct)
        {
            return await noteRepository.GetAllAsync(userId, ct);
        }

        public async Task<Note> CreateNoteAsync(string userId, string title, string description, string color, CancellationToken ct)
        {
            // Entity creation moved to Repository/Infrastructure layer just like User module!
            return await noteRepository.CreateAsync(userId, title, description, color, ct);
        }

        public async Task<Note?> UpdateNoteAsync(Guid id, string userId, string title, string description, string color, bool isPinned, bool isArchived, CancellationToken ct)
        {
            return await noteRepository.UpdateAsync(id, userId, title, description, color, isPinned, isArchived, ct);
        }

        public async Task<bool> DeleteNoteAsync(Guid id, string userId, CancellationToken ct)
        {
            return await noteRepository.DeleteAsync(id, userId, ct);
        }
    }
}