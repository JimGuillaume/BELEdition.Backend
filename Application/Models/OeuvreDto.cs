namespace Application.Models;

public sealed record OeuvreDto(
    string Id,
    string Titre,
    string? Resume,
    string? CouverturePremiere,
    string? CouvertureQuatrieme,
    DateTime DateCreation,
    DateTime? DateModification);
