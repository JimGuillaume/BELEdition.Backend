using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class PasswordHasher : Application.Common.Interfaces.IPasswordHasher
{
    private readonly PasswordHasher<User> _inner = new();
    public string Hash(string p) => _inner.HashPassword(null!, p);
    public bool Verify(string h, string p) =>
        _inner.VerifyHashedPassword(null!, h, p) != PasswordVerificationResult.Failed;
}
