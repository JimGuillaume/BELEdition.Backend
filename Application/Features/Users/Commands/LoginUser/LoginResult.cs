namespace Application.Features.Users.Commands.LoginUser;

public enum LoginStatus { Ok, NonLie, Suspendu, Verrouille }

public sealed record LoginResult(LoginStatus Status, string? Token = null, string? Erreur = null)
{
    public static LoginResult Ok(string token) => new(LoginStatus.Ok, token);
    public static LoginResult NonLie() => new(LoginStatus.NonLie, Erreur: "Aucun compte lié à cet itsme.");
    public static LoginResult Suspendu() => new(LoginStatus.Suspendu, Erreur: "L'accès à ce compte est suspendu.");
    public static LoginResult Verrouille() => new(LoginStatus.Verrouille, Erreur: "Compte temporairement verrouillé.");
}