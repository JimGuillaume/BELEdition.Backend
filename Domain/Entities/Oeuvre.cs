using Domain.Common;

namespace Domain.Entities;

public class Oeuvre : AuditableEntity
{
    public required string Titre { get; set; }

    public string? Resume { get; set; }

    // Couvertures : chemin ou URL de l'image
    public string? CouverturePremiere { get; set; }

    public string? CouvertureQuatrieme { get; set; }
}
