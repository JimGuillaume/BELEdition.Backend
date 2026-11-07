using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterRequest req, CancellationToken ct);
    Task<AuthResult> LoginAsync(LoginRequest req, CancellationToken ct);
}

public class AuthService(IApplicationDbContext db, IPasswordHasher hasher) : IAuthService
{
    private const string ErreurIdentifiants = "E-mail ou mot de passe incorrect.";
    private static string? _dummyHash;

    public async Task<AuthResult> RegisterAsync(RegisterRequest req, CancellationToken ct)
    {
        var erreurMdp = ValiderMotDePasse(req.MotDePasse);
        if (erreurMdp is not null) return AuthResult.Echec(erreurMdp);

        var email = (req.Email ?? "").Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(u => u.email == email, ct))
            return AuthResult.Echec("Un compte existe déjà avec cette adresse e-mail.");

        User user;
        try
        {
            user = User.CreerAvecMotDePasse(req.Nom, req.Prenom, email, hasher.Hash(req.MotDePasse));
        }
        catch (ArgumentException ex)
        {
            return AuthResult.Echec(ex.Message);
        }

        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        return AuthResult.Ok(user);
    }

    public async Task<AuthResult> LoginAsync(LoginRequest req, CancellationToken ct)
    {
        var email = (req.Email ?? "").Trim().ToLowerInvariant();
        var user = await db.Users.FirstOrDefaultAsync(
            u => u.email == email && u.date_suppression == null, ct);
         
        if (user is null || user.mot_de_passe_hash is null)
        {
            _dummyHash ??= hasher.Hash("dummy-password-for-timing");
            hasher.Verify(_dummyHash, req.MotDePasse ?? "");
            return AuthResult.Echec(ErreurIdentifiants);
        }

        if (user.verrouille_jusqua > DateTime.UtcNow)
            return AuthResult.Echec("Compte temporairement verrouillé. Réessayez plus tard.");

        if (!hasher.Verify(user.mot_de_passe_hash, req.MotDePasse ?? ""))
        {
            user.UpdateTentatives_echouees();
            await db.SaveChangesAsync(ct);
            return AuthResult.Echec(ErreurIdentifiants);
        }

        // Checked only after a correct password, so it reveals nothing to an attacker
        if (!user.est_actif || user.est_suspendu)
            return AuthResult.Echec("L'accès à ce compte est suspendu.");

        user.ResetTentatives_echouees();
        await db.SaveChangesAsync(ct);
        return AuthResult.Ok(user);
    }


    /// --- password validation ---
    /// a decider type de mot de passe (complexité, longueur, etc.) et si on veut le faire côté client ou côté serveur
    private static string? ValiderMotDePasse(string? mdp)
    {
        if (string.IsNullOrEmpty(mdp) || mdp.Length < 12)
            return "Le mot de passe doit contenir au moins 12 caractères.";
        if (!mdp.Any(char.IsUpper) || !mdp.Any(char.IsLower) || !mdp.Any(char.IsDigit))
            return "Le mot de passe doit contenir une majuscule, une minuscule et un chiffre.";
        return null;
    }
}