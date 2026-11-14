using Application.Common.Messaging;
using Application.Features.Users.Commands.LoginUser;
using Application.Features.Users.Commands.RegisterUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IDispatcher dispatcher) : ControllerBase
{
    public sealed record ItsmeLoginRequest(string Code);
    public sealed record TokenResponse(string Token);

    [HttpPost("itsme")]
    [AllowAnonymous]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status423Locked)]
    public async Task<IActionResult> LoginWithItsme(ItsmeLoginRequest body, CancellationToken ct)
    {
        var result = await dispatcher.Send(new LoginWithItsmeCommand(body.Code), ct);

        return result.Status switch
        {
            LoginStatus.Ok => Ok(new TokenResponse(result.Token!)),
            LoginStatus.NonLie => Problem(result.Erreur, statusCode: StatusCodes.Status404NotFound),
            LoginStatus.Suspendu => Problem(result.Erreur, statusCode: StatusCodes.Status403Forbidden),
            LoginStatus.Verrouille => Problem(result.Erreur, statusCode: StatusCodes.Status423Locked),
            _ => Unauthorized(),
        };
    }

    // Quick check that the token is accepted
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me() => Ok(new
    {
        Id = User.FindFirstValue("sub"),
        Email = User.FindFirstValue("email"),
    });

    public sealed record RegisterRequest(string Nom, string Prenom, string Email, string MotDePasse);
    public sealed record LoginRequest(string Email, string MotDePasse);

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest b, CancellationToken ct)
    {
        var r = await dispatcher.Send(new RegisterUserCommand(b.Nom, b.Prenom, b.Email, b.MotDePasse), ct);
        return r.Status switch
        {
            RegisterStatus.Ok => Created($"/api/users/{r.User!.Id}", r.User),
            RegisterStatus.EmailPris => Problem(r.Erreur, statusCode: 409),
            _ => Problem(r.Erreur, statusCode: 400),
        };
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest b, CancellationToken ct)
    {
        var r = await dispatcher.Send(new LoginUserCommand(b.Email, b.MotDePasse), ct);
        return r.Status switch
        {
            LoginStatus.Ok => Ok(new TokenResponse(r.Token!)),
            LoginStatus.Suspendu => Problem(r.Erreur, statusCode: 403),
            LoginStatus.Verrouille => Problem(r.Erreur, statusCode: 423),
            _ => Problem(r.Erreur, statusCode: 401),
        };
    }
}