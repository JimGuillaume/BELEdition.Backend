using System.Data;
using Application.Common.Interfaces;
using Dapper;
using Domain.Entities;

namespace Persistence.Repositories;

public sealed class OeuvreRepository(IDbConnection connection) : IOeuvreRepository
{
    public async Task<Oeuvre?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var query = new CommandDefinition(
            commandText: """
                         SELECT Id, Titre, Resume, CouverturePremiere, CouvertureQuatrieme,
                                DateCreation, DateModification, DateSuppression,
                                Id_Autheur AS IdAuteur, Id_Type AS IdType, Id_Status AS IdStatus
                         FROM Oeuvres
                         WHERE Id = @Id AND DateSuppression IS NULL
                         """,
            parameters: new
            {
                Id = id
            },
            cancellationToken: cancellationToken);

        return await connection.QueryFirstOrDefaultAsync<Oeuvre>(query);
    }

    public async Task<IReadOnlyList<Oeuvre>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var query = new CommandDefinition(
            commandText: """
                         SELECT Id, Titre, Resume, CouverturePremiere, CouvertureQuatrieme,
                                DateCreation, DateModification, DateSuppression,
                                Id_Autheur AS IdAuteur, Id_Type AS IdType, Id_Status AS IdStatus
                         FROM Oeuvres
                         WHERE DateSuppression IS NULL
                         ORDER BY COALESCE(DateModification, DateCreation) DESC
                         """,
            cancellationToken: cancellationToken);

        var oeuvres = await connection.QueryAsync<Oeuvre>(query);

        return oeuvres.AsList();
    }

    public async Task<IReadOnlyList<Oeuvre>> GetByAuteurIdAsync(
        string idAuteur, CancellationToken cancellationToken = default)
    {
        var query = new CommandDefinition(
            commandText: """
                         SELECT Id, Titre, Resume, CouverturePremiere, CouvertureQuatrieme,
                                DateCreation, DateModification, DateSuppression,
                                Id_Autheur AS IdAuteur, Id_Type AS IdType, Id_Status AS IdStatus
                         FROM Oeuvres
                         WHERE Id_Autheur = @IdAuteur AND DateSuppression IS NULL
                         ORDER BY COALESCE(DateModification, DateCreation) DESC
                         """,
            parameters: new
            {
                IdAuteur = idAuteur
            },
            cancellationToken: cancellationToken);

        var oeuvres = await connection.QueryAsync<Oeuvre>(query);

        return oeuvres.AsList();
    }

    public async Task AddAsync(Oeuvre oeuvre, CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            commandText: """
                         INSERT INTO Oeuvres
                             (Id, Titre, Resume, CouverturePremiere, CouvertureQuatrieme,
                              DateCreation, Id_Autheur, Id_Type, Id_Status)
                         VALUES
                             (@Id, @Titre, @Resume, @CouverturePremiere, @CouvertureQuatrieme,
                              @DateCreation, @IdAuteur, @IdType, @IdStatus)
                         """,
            parameters: new
            {
                oeuvre.Id,
                oeuvre.Titre,
                oeuvre.Resume,
                oeuvre.CouverturePremiere,
                oeuvre.CouvertureQuatrieme,
                oeuvre.DateCreation,
                oeuvre.IdAuteur,
                oeuvre.IdType,
                oeuvre.IdStatus
            },
            cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }

    public async Task<bool> UpdateAsync(Oeuvre oeuvre, CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            commandText: """
                         UPDATE Oeuvres
                         SET Titre = @Titre,
                             Resume = @Resume,
                             CouverturePremiere = @CouverturePremiere,
                             CouvertureQuatrieme = @CouvertureQuatrieme,
                             DateModification = @DateModification
                         WHERE Id = @Id AND DateSuppression IS NULL
                         """,
            parameters: new
            {
                oeuvre.Id,
                oeuvre.Titre,
                oeuvre.Resume,
                oeuvre.CouverturePremiere,
                oeuvre.CouvertureQuatrieme,
                oeuvre.DateModification
            },
            cancellationToken: cancellationToken);

        return await connection.ExecuteAsync(command) > 0;
    }

    public async Task<bool> DeleteAsync(string id, DateTime dateSuppression, CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            commandText: """
                         UPDATE Oeuvres
                         SET DateSuppression = @DateSuppression
                         WHERE Id = @Id AND DateSuppression IS NULL
                         """,
            parameters: new
            {
                Id = id,
                DateSuppression = dateSuppression
            },
            cancellationToken: cancellationToken);

        return await connection.ExecuteAsync(command) > 0;
    }
}
