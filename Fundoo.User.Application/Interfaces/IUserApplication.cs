namespace Fundoo.User.Application.Interfaces;

public interface IUserApplication
{
    Task<object> RegisterAsync(string firstName, string lastName, string email, string password, CancellationToken ct = default);
    Task<object> LoginAsync(string email, string password, CancellationToken ct = default);
}