using Fundoo.User.Domain;

namespace Fundoo.User.Application.Interfaces;

public interface ITokenService
{
    string CreateAccessToken(AppUser user);
}