using Fundoo.Notes.Application.Interfaces;
using Fundoo.Notes.Domain.Entities;
using Fundoo.Notes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.Notes.Infrastructure.Repositories
{
    public class NoteRepository(NotesDbContext context) : INoteRepository
    {
        public async Task<IEnumerable<Note>> GetAllAsync(string userId, CancellationToken ct)
        {
            return await context.Notes
                .Where(n => n.UserId == userId)
                .ToListAsync(ct);
        }

        public async Task<Note?> GetByIdAsync(Guid id, string userId, CancellationToken ct)
        {
            return await context.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId, ct);
        }

        public async Task<Note> CreateAsync(string userId, string title, string description, string color, CancellationToken ct)
        {
            // Entity object creation strictly inside Infrastructure layer
            var note = new Note
            {
                UserId = userId,
                Title = title,
                Description = description,
                Color = color ?? "#FFFFFF"
            };

            await context.Notes.AddAsync(note, ct);
            await context.SaveChangesAsync(ct);
            return note;
        }

        public async Task<Note?> UpdateAsync(Guid id, string userId, string title, string description, string color, bool isPinned, bool isArchived, CancellationToken ct)
        {
            var note = await GetByIdAsync(id, userId, ct);
            if (note == null) return null;

            note.Title = title;
            note.Description = description;
            note.Color = color;
            note.IsPinned = isPinned;
            note.IsArchived = isArchived;
            note.UpdatedAtUtc = DateTime.UtcNow;

            context.Notes.Update(note);
            await context.SaveChangesAsync(ct);
            return note;
        }

        public async Task<bool> DeleteAsync(Guid id, string userId, CancellationToken ct)
        {
            var note = await GetByIdAsync(id, userId, ct);
            if (note == null) return false;

            context.Notes.Remove(note);
            await context.SaveChangesAsync(ct);
            return true;
        }
    }
}