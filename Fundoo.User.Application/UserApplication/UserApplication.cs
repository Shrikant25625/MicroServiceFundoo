using Fundoo.User.Application.Interfaces;

namespace Fundoo.User.Application.UserApplication;

public class UserApplication(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService) : IUserApplication
{
    public async Task<object> RegisterAsync(string firstName, string lastName, string email, string password, CancellationToken ct = default)
    {
        var normalizedEmail = email.Trim().ToUpperInvariant();

        var existingUser = await userRepository.GetByNormalizedEmailAsync(normalizedEmail, ct);
        if (existingUser != null)
        {
            throw new Exception("Email is already registered.");
        }

        var passwordHash = passwordHasher.Hash(password);

        // Make sure await is used here so 'user' becomes AppUser, not Task<AppUser>
        var user = await userRepository.CreateUserAsync(firstName, lastName, email, normalizedEmail, passwordHash, ct);
        await userRepository.SaveChangesAsync(ct);

        var token = tokenService.CreateAccessToken(user);
        return new { userId = user.Id, email = user.Email, token, message = "Registration successful" };
    }

    public async Task<object> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var normalizedEmail = email.Trim().ToUpperInvariant();
        var user = await userRepository.GetByNormalizedEmailAsync(normalizedEmail, ct);

        if (user == null || !passwordHasher.Verify(password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = tokenService.CreateAccessToken(user);
        return new { userId = user.Id, email = user.Email, token, message = "Login successful" };
    }
}