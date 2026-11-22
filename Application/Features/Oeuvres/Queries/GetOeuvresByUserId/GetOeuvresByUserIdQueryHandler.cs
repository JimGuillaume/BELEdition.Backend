using Application.Common.Interfaces;
using Application.Common.Messaging;
using Application.Mappers;
using Application.Models;

namespace Application.Features.Oeuvres.Queries.GetOeuvresByUserId;

public sealed class GetOeuvresByUserIdQueryHandler(IOeuvreRepository oeuvreRepository)
    : IRequestHandler<GetOeuvresByUserIdQuery, IReadOnlyList<OeuvreDto>>
{
    public async Task<IReadOnlyList<OeuvreDto>> Handle(
        GetOeuvresByUserIdQuery request, CancellationToken cancellationToken)
    {
        var oeuvres = await oeuvreRepository.GetByAuteurIdAsync(request.UserId, cancellationToken);

        return oeuvres.ToDto().ToList();
    }
}
