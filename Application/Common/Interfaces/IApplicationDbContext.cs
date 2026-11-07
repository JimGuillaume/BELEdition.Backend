using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

// Implémenté par Persistence.ApplicationDbContext
public interface IApplicationDbContext
{
    DbSet<Oeuvre> Oeuvres { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
