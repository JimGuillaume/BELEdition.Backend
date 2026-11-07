using Application.Auth;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService auth, IJwtTokenGenerator tokens) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req, CancellationToken ct)
    {
        var r = await auth.RegisterAsync(req, ct);
        if (!r.Succes) return BadRequest(new { erreur = r.Erreur });
        return StatusCode(201, ToResponse(r.User!));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req, CancellationToken ct)
    {
        var r = await auth.LoginAsync(req, ct);
        if (!r.Succes) return Unauthorized(new { erreur = r.Erreur });

        return Ok(new
        {
            token = tokens.Generate(r.User!),
            utilisateur = ToResponse(r.User!),
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me() => Ok(new
    {
        id = User.FindFirst("sub")?.Value,
        email = User.FindFirst("email")?.Value,
        nom = User.FindFirst("Nom")?.Value,
    });

    private static UtilisateurResponse ToResponse(User u) =>
        new(u.id_utilisateur, u.nom, u.prenom, u.email);
}