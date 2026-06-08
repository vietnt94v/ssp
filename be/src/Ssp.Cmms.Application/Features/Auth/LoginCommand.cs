using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Application.Features.Auth;

public record LoginCommand(string Email, string Password)
    : IRequest<LoginResult>;

public record LoginUserDto(Guid Id, string Email, string FullName, Role Role);

public record LoginResult(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn,
    LoginUserDto User);

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IApplicationDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenService _tokens;

    public LoginCommandHandler(
        IApplicationDbContext db,
        IPasswordHasher hasher,
        IJwtTokenService tokens)
    {
        _db = db;
        _hasher = hasher;
        _tokens = tokens;
    }

    public async Task<LoginResult> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(
                u => u.Email == request.Email && u.IsActive,
                cancellationToken);

        if (user is null || !_hasher.Verify(request.Password, user.PasswordHash))
        {
            throw new Common.Exceptions.BusinessRuleException(
                "Invalid email or password.");
        }

        var accessToken = _tokens.CreateAccessToken(user);
        var refreshToken = _tokens.CreateRefreshToken();

        return new LoginResult(
            accessToken,
            refreshToken,
            3600,
            new LoginUserDto(user.Id, user.Email, user.FullName, user.Role));
    }
}
