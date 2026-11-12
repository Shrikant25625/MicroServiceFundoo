using Fundoo.User.Domain;

namespace Fundoo.User.Application.Interfaces;

public interface IUserRepository
{
    Task<AppUser?> GetByNormalizedEmailAsync(string normalizedEmail, CancellationToken ct = default);
    Task<AppUser> CreateUserAsync(string firstName, string lastName, string email, string normalizedEmail, string passwordHash, CancellationToken ct = default);
    Task<bool> SaveChangesAsync(CancellationToken ct = default);
}