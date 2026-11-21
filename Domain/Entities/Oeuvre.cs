using Domain.Common;

namespace Domain.Entities;

public class Oeuvre : AuditableEntity
{
    public required string Titre { get; set; }

    public string? Resume { get; set; }

    // Couvertures : chemin ou URL de l'image
    public string? CouverturePremiere { get; set; }

    public string? CouvertureQuatrieme { get; set; }

    // Clés étrangères (colonnes Id_Autheur / Id_Type / Id_Status)
    public required string IdAuteur { get; set; }

    public required string IdType { get; set; }

    public required string IdStatus { get; set; }
}
