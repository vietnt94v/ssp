using Ssp.Cmms.Domain.Entities;

namespace Ssp.Cmms.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string CreateAccessToken(User user);
    string CreateRefreshToken();
}

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
