using Application.Common.Messaging;
using Application.Models;

namespace Application.Features.Oeuvres.Queries.GetOeuvre;

public sealed record GetOeuvreQuery(string Id) : IRequest<OeuvreDto?>;
