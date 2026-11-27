using Application.Common.Messaging;
using Application.Models;

namespace Application.Features.Oeuvres.Queries.GetAllOeuvres;

public sealed record GetAllOeuvresQuery : IRequest<IReadOnlyList<OeuvreDto>>;
