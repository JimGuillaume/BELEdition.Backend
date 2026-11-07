using FluentMigrator;

namespace Persistence.Migrations;

// Données de test pour GET /api/oeuvres/{id}
[Migration(202611071300)]
public class SeedOeuvres : Migration
{
    private const string OeuvreId1 = "01JD0000000000000000000001";
    private const string OeuvreId2 = "01JD0000000000000000000002";
    private const string OeuvreId3 = "01JD0000000000000000000003";

    // Placeholders tant que les tables Auteur / Type / Status n'existent pas
    private const string AuteurId = "01JD00000000000000000000A1";
    private const string TypeId = "01JD00000000000000000000T1";
    private const string StatusId = "01JD00000000000000000000S1";

    public override void Up()
    {
        var now = DateTime.UtcNow;

        Insert.IntoTable("Oeuvres")
            .Row(new
            {
                Id = OeuvreId1,
                Titre = "Les Ombres de Bruxelles",
                Resume = "Un polar haletant dans les ruelles de la capitale.",
                CouverturePremiere = "https://picsum.photos/seed/oeuvre1/400/600",
                CouvertureQuatrieme = "https://picsum.photos/seed/oeuvre1b/400/600",
                DateCreation = now,
                Id_Autheur = AuteurId,
                Id_Type = TypeId,
                Id_Status = StatusId,
            })
            .Row(new
            {
                Id = OeuvreId2,
                Titre = "Le Jardin des Mots",
                Resume = "Recueil de poèmes sur le passage des saisons.",
                CouverturePremiere = "https://picsum.photos/seed/oeuvre2/400/600",
                DateCreation = now,
                Id_Autheur = AuteurId,
                Id_Type = TypeId,
                Id_Status = StatusId,
            })
            .Row(new
            {
                Id = OeuvreId3,
                Titre = "Sans Couverture",
                DateCreation = now,
                Id_Autheur = AuteurId,
                Id_Type = TypeId,
                Id_Status = StatusId,
            });
    }

    public override void Down()
    {
        Delete.FromTable("Oeuvres")
            .Row(new { Id = OeuvreId1 })
            .Row(new { Id = OeuvreId2 })
            .Row(new { Id = OeuvreId3 });
    }
}
