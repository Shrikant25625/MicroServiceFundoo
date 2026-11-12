using Fundoo.User.Application.Interfaces;
using Fundoo.User.Domain;
using Fundoo.User.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.User.Infrastructure.Repository;

public class UserRepository(UserDbContext context) : IUserRepository
{
    public async Task<AppUser?> GetByNormalizedEmailAsync(string normalizedEmail, CancellationToken ct = default)
        => await context.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, ct);

    public async Task<AppUser> CreateUserAsync(string firstName, string lastName, string email, string normalizedEmail, string passwordHash, CancellationToken ct = default)
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            NormalizedEmail = normalizedEmail,
            PasswordHash = passwordHash,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        await context.Users.AddAsync(user, ct);
        return user;
    }

    public async Task<bool> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct) > 0;
}