using Application.Common.Messaging;
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

}
