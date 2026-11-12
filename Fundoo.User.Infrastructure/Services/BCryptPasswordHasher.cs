using Fundoo.User.Application.Interfaces;

namespace Fundoo.User.Infrastructure.Services
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string providedPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}