using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Utilisateur");

        b.HasKey(u => u.id_utilisateur);
        b.Property(u => u.id_utilisateur).UseIdentityColumn();

        b.Property(u => u.nom).HasMaxLength(100).IsRequired();
        b.Property(u => u.prenom).HasMaxLength(100).IsRequired();
        b.Property(u => u.email).HasMaxLength(255).IsRequired();
        b.Property(u => u.mot_de_passe_hash).HasMaxLength(255);      // NULL = itsme-only account
        b.Property(u => u.eid_identifiant_hash).HasMaxLength(255);  // NULL = password-only account

        b.Property(u => u.est_actif).HasDefaultValue(true).HasSentinel(true);
        b.Property(u => u.est_suspendu).HasDefaultValue(false);
        b.Property(u => u.tentatives_echouees).HasDefaultValue(0);

        b.Property(u => u.date_inscription).IsRequired();
        b.Property(u => u.verrouille_jusqua);
        b.Property(u => u.date_suppression);

        b.HasIndex(u => u.email).IsUnique();
        b.HasIndex(u => u.eid_identifiant_hash).IsUnique();
    }
}