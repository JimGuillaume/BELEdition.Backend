using Application.Common.Messaging;
using Application.Features.Oeuvres.Queries.GetOeuvre;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/oeuvres")]
public sealed class OeuvresController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType<OeuvreDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOeuvre(string id, CancellationToken cancellationToken)
    {
        var oeuvre = await dispatcher.Send(new GetOeuvreQuery(id), cancellationToken);

        return oeuvre is null ? NotFound() : Ok(oeuvre);
    }
}
