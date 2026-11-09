using Application.Common.Messaging;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Queries.GetUser;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(string id, CancellationToken cancellationToken)
    {
        var user = await dispatcher.Send(new GetUserQuery(id), cancellationToken);

        return user is null ? NotFound() : Ok(user);
    }

    [HttpGet]
    [ProducesResponseType<UserDto[]>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllActiveUsers(CancellationToken cancellationToken)
    => Ok(await dispatcher.Send(new GetAllActiveUsersQuery(), cancellationToken));

    [HttpGet("all")]
    // TODO: [Authorize(Roles = "Administrateur")] quand l'auth sera en place
    [ProducesResponseType<UserDto[]>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        => Ok(await dispatcher.Send(new GetAllUsersQuery(), cancellationToken));

    public sealed record UpdateUserRequest(string Nom, string Prenom, string Email, string? MotdePasseHash, bool? EstActif, bool? EstSuspendu);

    [HttpPut("{id}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserRequest body, CancellationToken cancellationToken)
    {
        var user = await dispatcher.Send(
            new UpdateUserCommand(id, body.Nom, body.Prenom, body.Email, body.MotdePasseHash, body.EstActif, body.EstSuspendu), cancellationToken);

        return user is null ? NotFound() : Ok(user);
    }

    public sealed record CreateUserRequest(string Nom, string Prenom, string Email, string? MotDePasseHash, string? EidIdentifiantHash);

    [HttpPost]
    [ProducesResponseType<UserDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUser(CreateUserRequest body, CancellationToken cancellationToken)
    {
        var user = await dispatcher.Send(
            new CreateUserCommand(body.Nom, body.Prenom, body.Email, body.MotDePasseHash, body.EidIdentifiantHash),
            cancellationToken);

        return user is null
            ? Conflict("Cet email est déjà utilisé.")
            : CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }
}
