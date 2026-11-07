using System.Data;
using Application.Common.Interfaces;
using Microsoft.Data.SqlClient;

namespace Persistence;

public class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    // Connexion fermée : Dapper l'ouvre et la ferme à chaque requête
    public IDbConnection CreateConnection() => new SqlConnection(connectionString);
}
