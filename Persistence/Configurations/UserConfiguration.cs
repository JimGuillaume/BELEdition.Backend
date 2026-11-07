using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.Property(u => u.email).HasMaxLength(256).IsRequired();
        b.Property(u => u.nom).HasMaxLength(50).IsRequired();
        b.Property(u => u.prenom).HasMaxLength(50).IsRequired();
        b.Property(u => u.mot_de_passe_hash).IsRequired();
        b.Property(u => u.est_actif).IsRequired();
        b.Property(u => u.est_suspendu).IsRequired();
        b.Property(u => u.tentatives_echouees).IsRequired();
        b.Property(u => u.date_inscription).IsRequired();
        b.Property(u => u.eid_identifiant_hash);
        b.Property(u => u.verrouille_jusqua);
        b.Property(u => u.date_suppression);
        b.HasIndex(u => u.email).IsUnique();
        b.HasIndex(u => u.eid_identifiant_hash).IsUnique();
    }
}