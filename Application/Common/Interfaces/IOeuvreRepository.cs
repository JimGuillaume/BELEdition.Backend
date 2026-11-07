using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IOeuvreRepository
{
    // Retourne null si absente ou supprimée
    Task<Oeuvre?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
