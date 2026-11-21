using Application.Common.Messaging;
using Application.Features.Oeuvres.Commands.CreateOeuvre;
using Application.Features.Oeuvres.Commands.DeleteOeuvre;
using Application.Features.Oeuvres.Commands.UpdateOeuvre;
using Application.Features.Oeuvres.Queries.GetOeuvre;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/oeuvre")]
public sealed class OeuvreController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType<OeuvreDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOeuvre(string id, CancellationToken cancellationToken)
    {
        var oeuvre = await dispatcher.Send(new GetOeuvreQuery(id), cancellationToken);

        return oeuvre is null ? NotFound() : Ok(oeuvre);
    }

    [HttpPost]
    [ProducesResponseType<OeuvreDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOeuvre(CreateOeuvreCommand command, CancellationToken cancellationToken)
    {
        var oeuvre = await dispatcher.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetOeuvre), new { id = oeuvre.Id }, oeuvre);
    }

    [HttpPut("{id}")]
    [ProducesResponseType<OeuvreDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOeuvre(
        string id, UpdateOeuvreRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateOeuvreCommand(
            id,
            request.Titre,
            request.Resume,
            request.CouverturePremiere,
            request.CouvertureQuatrieme);

        var oeuvre = await dispatcher.Send(command, cancellationToken);

        return oeuvre is null ? NotFound() : Ok(oeuvre);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOeuvre(string id, CancellationToken cancellationToken)
    {
        var deleted = await dispatcher.Send(new DeleteOeuvreCommand(id), cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}
