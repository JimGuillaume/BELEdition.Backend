using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<User[]> GetAllAsync(CancellationToken ct = default);
    Task<User[]> GetAllActiveAsync(CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<User?> GetByEidHashAsync(string eidHash, CancellationToken ct = default);  //  worth keeping?
    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    Task UpdateAsync(User user, CancellationToken ct = default);
}