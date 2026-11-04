using System.Reflection;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    // Ajouter ici les DbSet<TEntity> des entités du Domain

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Applique automatiquement toutes les IEntityTypeConfiguration<T> du dossier Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
