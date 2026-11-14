using System.Security.Claims;
using Application.Common.Messaging;
using Application.Features.Users.Commands.LoginUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
}