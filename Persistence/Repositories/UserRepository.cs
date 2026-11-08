using Application.Common.Interfaces;
using Dapper;
using Domain.Entities;
using System.Data;

public sealed class UserRepository(IDbConnection connection) : IUserRepository
{
    private const string SelectActive = "SELECT * FROM Utilisateurs WHERE DateSuppression IS NULL";

    //public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    //{
    //    var query = new CommandDefinition(
    //        commandText: """
    //                     {SelectActive} and Id = @id
    //                     """,
    //        parameters: new
    //        {
    //            Id = id
    //        },
    //        cancellationToken: cancellationToken);

    //    return await connection.QueryFirstOrDefaultAsync<User>(query);
    //}

    public Task<User?> GetByIdAsync(string id, CancellationToken ct = default) =>
        connection.QueryFirstOrDefaultAsync<User>(new CommandDefinition(
            $"{SelectActive} AND Id = @id", new { id }, cancellationToken: ct));
    public async Task<User[]> GetAllAsync(CancellationToken ct = default) =>
       (await connection.QueryAsync<User>(new CommandDefinition(
           $"SELECT * FROM Utilisateurs", cancellationToken: ct))).ToArray();
    public async Task<User[]> GetAllActiveAsync(CancellationToken ct = default) =>
       (await connection.QueryAsync<User>(new CommandDefinition(
           SelectActive, cancellationToken: ct))).ToArray();

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        connection.QueryFirstOrDefaultAsync<User>(new CommandDefinition(
            $"{SelectActive} AND Email = @email", new { email = email.Trim().ToLowerInvariant() }, cancellationToken: ct));
    
    public Task<User?> GetByEidHashAsync(string eidHash, CancellationToken ct = default) =>
        connection.QueryFirstOrDefaultAsync<User>(new CommandDefinition(
            $"{SelectActive} AND EidIdentifiantHash = @eidHash", new { eidHash = eidHash.Trim().ToLowerInvariant() }, cancellationToken: ct));

    public Task<bool> EmailExistsAsync(string email, CancellationToken ct = default) =>
        connection.ExecuteScalarAsync<bool>(new CommandDefinition(
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM Utilisateurs WHERE Email = @email) THEN 1 ELSE 0 END",
            new { email = email.Trim().ToLowerInvariant() }, cancellationToken: ct));

    public Task AddAsync(User user, CancellationToken ct = default) =>
        connection.ExecuteAsync(new CommandDefinition("""
            INSERT INTO Utilisateurs (Id, Nom, Prenom, Email, MotDePasseHash, EidIdentifiantHash,
                EstActif, EstSuspendu, TentativesEchouees, VerrouilleJusqua, DateCreation)
            VALUES (@Id, @Nom, @Prenom, @Email, @MotDePasseHash, @EidIdentifiantHash,
                @EstActif, @EstSuspendu, @TentativesEchouees, @VerrouilleJusqua, @DateCreation)
            """, user, cancellationToken: ct));

    public Task UpdateAsync(User user, CancellationToken ct = default) =>
        connection.ExecuteAsync(new CommandDefinition("""
            UPDATE Utilisateurs SET Nom=@Nom, Prenom=@Prenom, Email=@Email, MotDePasseHash=@MotDePasseHash,
                EidIdentifiantHash=@EidIdentifiantHash, EstActif=@EstActif, EstSuspendu=@EstSuspendu,
                TentativesEchouees=@TentativesEchouees, VerrouilleJusqua=@VerrouilleJusqua,
                DateSuppression=@DateSuppression, DateModification=SYSUTCDATETIME()
            WHERE Id=@Id
            """, user, cancellationToken: ct));
}