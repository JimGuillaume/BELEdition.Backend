using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Security;

// Infrastructure/Security/FakeItsmeClient.cs
public sealed class FakeItsmeClient : IItsmeClient
{
    public Task<string> ExchangeCodeAsync(string code, CancellationToken ct) => Task.FromResult(code); // code = sub
}
