using Application.Common.Interfaces;
using Application.Common.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Commands.LoginUser;
public sealed class LoginUserCommandHandler(IUserRepository users, IPasswordHasher hasher, IJwtTokenGenerator jwt)
    : IRequestHandler<LoginUserCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginUserCommand r, CancellationToken ct)
    {
        var user = await users.GetByEmailAsync(r.Email.Trim().ToLowerInvariant(), ct);

        if (user?.MotDePasseHash is null)
        {
            hasher.Verify(_dummy ??= hasher.Hash("dummy"), r.MotDePasse); // same timing as a real check
            return LoginResult.Invalide();
        }

        // Check BEFORE verifying: UpdateTentativesEchouees() throws if already locked
        if (user.VerrouilleJusqua > DateTime.UtcNow) return LoginResult.Verrouille();

        var check = hasher.Verify(user.MotDePasseHash, r.MotDePasse);
        if (check == PasswordCheck.Echec)
        {
            user.UpdateTentativesEchouees();
            await users.UpdateAsync(user, ct);
            return LoginResult.Invalide();
        }

        // Only after a correct password, so it reveals nothing
        if (!user.EstActif || user.EstSuspendu) return LoginResult.Suspendu();

        user.ResetTentativesEchouees();
        if (check == PasswordCheck.OkRehash) user.UpdatePassword(hasher.Hash(r.MotDePasse));
        await users.UpdateAsync(user, ct);

        return LoginResult.Ok(jwt.Generate(user));
    }
    private static string? _dummy;
}
