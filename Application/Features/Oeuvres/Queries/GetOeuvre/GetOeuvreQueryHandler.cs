using Application.Common.Interfaces;
using Application.Common.Messaging;
using Application.Mappers;
using Application.Models;

namespace Application.Features.Oeuvres.Queries.GetOeuvre;

public sealed class GetOeuvreQueryHandler(IOeuvreRepository oeuvreRepository)
    : IRequestHandler<GetOeuvreQuery, OeuvreDto?>
{
    public async Task<OeuvreDto?> Handle(GetOeuvreQuery request, CancellationToken cancellationToken)
    {
        var oeuvre = await oeuvreRepository.GetByIdAsync(request.Id, cancellationToken);

        return oeuvre?.ToDto();
    }
}
