using Domain.Entities;

namespace Application.Auth;

public sealed record RegisterRequest(string Nom, string Prenom, string Email, string MotDePasse);
public sealed record LoginRequest(string Email, string MotDePasse);
public sealed record UtilisateurResponse(int Id, string Nom, string Prenom, string Email);

public sealed record AuthResult(bool Succes, string? Erreur = null, User? User = null)
{
    public static AuthResult Ok(User u) => new(true, null, u);
    public static AuthResult Echec(string e) => new(false, e);
}