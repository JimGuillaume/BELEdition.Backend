using Application.Common.Interfaces;
using Application.Common.Messaging;

namespace Application.Features.Users.Commands.LoginUser;

public sealed class LoginWithItsmeCommandHandler(
    IItsmeClient itsme, IEidHasher hasher, IUserRepository users, IJwtTokenGenerator jwt)
    : IRequestHandler<LoginWithItsmeCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginWithItsmeCommand request, CancellationToken ct)
    {
        var sub = await itsme.ExchangeCodeAsync(request.Code, ct);
        var user = await users.GetByEidHashAsync(hasher.Hash(sub), ct);

        if (user is null) return LoginResult.NonLie();
        if (user.EstSuspendu) return LoginResult.Suspendu();
        if (user.VerrouilleJusqua > DateTime.UtcNow) return LoginResult.Verrouille();

        return LoginResult.Ok(jwt.Generate(user));
    }
}