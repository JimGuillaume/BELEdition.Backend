namespace Domain.Common;

// Dates en UTC
public abstract class AuditableEntity : BaseEntity
{
    public DateTime DateCreation { get; set; }

    public DateTime? DateModification { get; set; }

    // Non null = supprimée (soft delete)
    public DateTime? DateSuppression { get; set; }

    public bool EstSupprime => DateSuppression.HasValue;
}
