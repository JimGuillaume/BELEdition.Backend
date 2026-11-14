using Application.Common.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Security;

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
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Nom", user.Nom)
                ]),
            Issuer = s.Issuer,
            Audience = s.Audience,
            Expires = DateTime.UtcNow.AddMinutes(s.ExpiresMinutes),
            SigningCredentials = creds
        });
    }
}