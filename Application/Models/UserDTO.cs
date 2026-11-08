namespace Application.Models;

public sealed record UserDto(
    string Id,
    string Nom,
    string Prenom,
    string Email,
    //string? MotDePasseHash,
    //string? EidIdentifiantHash,
    bool EstActif,
    bool EstSuspendu,
    int TentativesEchouees,
    DateTime? VerrouilleJusqua,
    DateTime DateCreation,
    DateTime? DateSuppression
    );
