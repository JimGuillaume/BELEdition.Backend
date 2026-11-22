using Application.Common.Messaging;
using Application.Models;

namespace Application.Features.Oeuvres.Queries.GetOeuvresByUserId;

public sealed record GetOeuvresByUserIdQuery(string UserId) : IRequest<IReadOnlyList<OeuvreDto>>;
