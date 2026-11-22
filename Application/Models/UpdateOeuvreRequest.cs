namespace Application.Models;

// Corps de PUT /api/oeuvres/{id} (l'Id vient de la route)
public sealed record UpdateOeuvreRequest(
    string Titre,
    string? Resume,
    string? CouverturePremiere,
    string? CouvertureQuatrieme);
