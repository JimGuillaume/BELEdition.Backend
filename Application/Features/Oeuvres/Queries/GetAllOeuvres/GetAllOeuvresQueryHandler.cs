using Application.Common.Interfaces;
using Application.Common.Messaging;
using Application.Mappers;
using Application.Models;

namespace Application.Features.Oeuvres.Queries.GetAllOeuvres;

public sealed class GetAllOeuvresQueryHandler(IOeuvreRepository oeuvreRepository)
    : IRequestHandler<GetAllOeuvresQuery, IReadOnlyList<OeuvreDto>>
{
    public async Task<IReadOnlyList<OeuvreDto>> Handle(
        GetAllOeuvresQuery request, CancellationToken cancellationToken)
    {
        var oeuvres = await oeuvreRepository.GetAllAsync(cancellationToken);

        return oeuvres.ToDto().ToList();
    }
}
