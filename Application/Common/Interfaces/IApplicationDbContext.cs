using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

/// <summary>
/// Abstraction du DbContext utilisée par la couche Application.
/// L'implémentation (EF Core) se trouve dans le projet Persistence.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
