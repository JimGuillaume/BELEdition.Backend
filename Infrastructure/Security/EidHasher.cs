using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Security;

public sealed class EidHasher(IConfiguration config) : IEidHasher
{
    private readonly byte[] _key = Convert.FromBase64String(
        config["Itsme:EidHashKey"] ?? throw new InvalidOperationException("Itsme:EidHashKey manquant."));

    public string Hash(string sub) =>
        Convert.ToHexStringLower(HMACSHA256.HashData(_key, Encoding.UTF8.GetBytes(sub.Trim())));
}