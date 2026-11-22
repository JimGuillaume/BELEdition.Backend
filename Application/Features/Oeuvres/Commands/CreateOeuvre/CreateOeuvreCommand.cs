using Application.Common.Messaging;
using Application.Models;

namespace Application.Features.Oeuvres.Commands.CreateOeuvre;

public sealed record CreateOeuvreCommand(
    string Titre,
    string? Resume,
    string? CouverturePremiere,
    string? CouvertureQuatrieme,
    string IdAuteur,
    string IdType,
    string IdStatus) : IRequest<OeuvreDto>;
