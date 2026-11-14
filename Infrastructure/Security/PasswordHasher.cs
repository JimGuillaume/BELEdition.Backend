using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _inner = new();

    public string Hash(string motDePasse) => _inner.HashPassword(null!, motDePasse);

    public PasswordCheck Verify(string hash, string motDePasse) =>
        _inner.VerifyHashedPassword(null!, hash, motDePasse) switch
        {
            PasswordVerificationResult.Success => PasswordCheck.Ok,
            PasswordVerificationResult.SuccessRehashNeeded => PasswordCheck.OkRehash,
            _ => PasswordCheck.Echec,
        };
}