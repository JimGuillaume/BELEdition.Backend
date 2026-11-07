using Application.Common.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Text;

namespace Infrastructure.Services;

public class JwtTokenGenerator(IOptions<JwtSettings> opt) : IJwtTokenGenerator
{
    public string Generate(User user)
    {
        var s = opt.Value;
        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s.Key)), SecurityAlgorithms.HmacSha256);

        return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.id_utilisateur.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.email),
            new Claim("Nom", user.nom)
            ]),
            Issuer = s.Issuer,
            Audience = s.Audience,
            Expires = DateTime.UtcNow.AddMinutes(s.ExpiresMinutes),
            SigningCredentials = creds
        });
    }
}
