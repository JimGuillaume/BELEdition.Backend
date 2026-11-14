using Application.Common.Interfaces;
using Application.Common.Messaging;
using Application.Mappers;
using Application.Models;
using Domain.Entities;

namespace Application.Features.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler(IUserRepository users, IPasswordHasher hasher)
    : IRequestHandler<RegisterUserCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterUserCommand r, CancellationToken ct)
    {
        var erreur = ValiderMotDePasse(r.MotDePasse);
        if (erreur is not null) return new(RegisterStatus.Invalide, Erreur: erreur);

        if (await users.EmailExistsAsync(r.Email.Trim().ToLowerInvariant(), ct))
            return new(RegisterStatus.EmailPris, Erreur: "Un compte existe déjà avec cette adresse e-mail.");

        User user;
        try { user = User.CreerAvecMotDePasse(r.Nom, r.Prenom, r.Email, hasher.Hash(r.MotDePasse)); }
        catch (ArgumentException ex) { return new(RegisterStatus.Invalide, Erreur: ex.Message); }

        await users.AddAsync(user, ct);
        return new(RegisterStatus.Ok, user.ToDto());
    }

    private static string? ValiderMotDePasse(string? mdp)
    {
        if (string.IsNullOrEmpty(mdp) || mdp.Length < 12)
            return "Le mot de passe doit contenir au moins 12 caractères.";
        if (!mdp.Any(char.IsUpper) || !mdp.Any(char.IsLower) || !mdp.Any(char.IsDigit))
            return "Le mot de passe doit contenir une majuscule, une minuscule et un chiffre.";
        return null;
    }
}