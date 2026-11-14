using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces;

public interface IItsmeClient
{
    // Exchanges the authorization code, validates the ID token, returns the "sub"
    Task<string> ExchangeCodeAsync(string code, CancellationToken ct);
}
