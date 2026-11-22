using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IOeuvreRepository
{
    // Retourne null si absente ou supprimée
    Task<Oeuvre?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    // Oeuvres non supprimées de l'auteur, plus récemment modifiées en premier
    Task<IReadOnlyList<Oeuvre>> GetByAuteurIdAsync(string idAuteur, CancellationToken cancellationToken = default);

    Task AddAsync(Oeuvre oeuvre, CancellationToken cancellationToken = default);

    // Retourne false si absente ou supprimée
    Task<bool> UpdateAsync(Oeuvre oeuvre, CancellationToken cancellationToken = default);

    // Soft delete ; retourne false si absente ou déjà supprimée
    Task<bool> DeleteAsync(string id, DateTime dateSuppression, CancellationToken cancellationToken = default);
}
