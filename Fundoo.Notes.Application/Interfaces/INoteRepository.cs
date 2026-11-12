using Fundoo.Notes.Domain.Entities;

namespace Fundoo.Notes.Application.Interfaces
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllAsync(string userId, CancellationToken ct);
        Task<Note?> GetByIdAsync(Guid id, string userId, CancellationToken ct);
        Task<Note> CreateAsync(string userId, string title, string description, string color, CancellationToken ct);
        Task<Note?> UpdateAsync(Guid id, string userId, string title, string description, string color, bool isPinned, bool isArchived, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, string userId, CancellationToken ct);
    }
}