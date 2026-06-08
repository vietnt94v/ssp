using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Application.Common.Interfaces;

public interface ICurrentUser
{
    string? UserId { get; }
    string? Email { get; }
    Role? Role { get; }
    bool IsAuthenticated { get; }
}
