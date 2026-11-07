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
                         SELECT * FROM Oeuvres 
                         WHERE Id = @Id
                         """,
            parameters: new
            {
                Id = id
            },
            cancellationToken: cancellationToken);

        return await connection.QueryFirstOrDefaultAsync<Oeuvre>(query);
    }
}
