using System.Data;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Seeds;

// Données de test, à n'exécuter qu'en développement. Idempotent : n'insère que les lignes absentes.
public static class DevelopmentSeeder
{
    // Ids fixes pour pouvoir tester GET /api/oeuvres/{id}
    public const string OeuvreId1 = "01JD0000000000000000000001";
    public const string OeuvreId2 = "01JD0000000000000000000002";
    public const string OeuvreId3 = "01JD0000000000000000000003";
    public const string OeuvreSupprimeeId = "01JD0000000000000000000004";

    // Placeholders tant que les tables Auteur / Type / Status n'existent pas
    private const string AuteurId = "01JD00000000000000000000A1";
    private const string TypeId = "01JD00000000000000000000T1";
    private const string StatusId = "01JD00000000000000000000S1";

    public static IServiceProvider SeedDevelopmentData(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

        var now = DateTime.UtcNow;

        var oeuvres = new[]
        {
            new
            {
                Id = OeuvreId1,
                Titre = "Les Ombres de Bruxelles",
                Resume = (string?)"Un polar haletant dans les ruelles de la capitale.",
                CouverturePremiere = (string?)"https://picsum.photos/seed/oeuvre1/400/600",
                CouvertureQuatrieme = (string?)"https://picsum.photos/seed/oeuvre1b/400/600",
                DateCreation = now,
                DateSuppression = (DateTime?)null,
            },
            new
            {
                Id = OeuvreId2,
                Titre = "Le Jardin des Mots",
                Resume = (string?)"Recueil de poèmes sur le passage des saisons.",
                CouverturePremiere = (string?)"https://picsum.photos/seed/oeuvre2/400/600",
                CouvertureQuatrieme = (string?)null,
                DateCreation = now,
                DateSuppression = (DateTime?)null,
            },
            new
            {
                Id = OeuvreId3,
                Titre = "Sans Couverture",
                Resume = (string?)null,
                CouverturePremiere = (string?)null,
                CouvertureQuatrieme = (string?)null,
                DateCreation = now,
                DateSuppression = (DateTime?)null,
            },
            new
            {
                Id = OeuvreSupprimeeId,
                Titre = "Oeuvre Supprimée",
                Resume = (string?)"Soft delete : ne devrait pas être retournée.",
                CouverturePremiere = (string?)null,
                CouvertureQuatrieme = (string?)null,
                DateCreation = now,
                DateSuppression = (DateTime?)now,
            },
        };

        const string sql = """
            IF NOT EXISTS (SELECT 1 FROM Oeuvres WHERE Id = @Id)
                INSERT INTO Oeuvres
                    (Id, Titre, Resume, CouverturePremiere, CouvertureQuatrieme,
                     DateCreation, DateSuppression, Id_Autheur, Id_Type, Id_Status)
                VALUES
                    (@Id, @Titre, @Resume, @CouverturePremiere, @CouvertureQuatrieme,
                     @DateCreation, @DateSuppression, @AuteurId, @TypeId, @StatusId)
            """;

        foreach (var oeuvre in oeuvres)
        {
            connection.Execute(sql, new
            {
                oeuvre.Id,
                oeuvre.Titre,
                oeuvre.Resume,
                oeuvre.CouverturePremiere,
                oeuvre.CouvertureQuatrieme,
                oeuvre.DateCreation,
                oeuvre.DateSuppression,
                AuteurId,
                TypeId,
                StatusId,
            });
        }
    
        var users = new[]
        {
            new
            {
                Id = AuteurId,
                Nom = "Jean Dupont",
                Email = "jean.dupont@example.com",
                DateCreation = now,
                DateSuppression = (DateTime?)null,
            }
        };

        const string sql1 = """
            IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @Id)
                INSERT INTO Users
                    (Id, Nom, Email, DateCreation, DateSuppression)
                VALUES
                    (@Id, @Nom, @Email, @DateCreation, @DateSuppression)
            """;

        foreach (var user in users)
        {
            connection.Execute(sql1, new
            {
                user.Id,
                user.Nom,
                user.Email,
                user.DateCreation,
                user.DateSuppression,
            });
        }

        return services;
    }
}
