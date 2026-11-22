using Application.Common.Messaging;
using Application.Models;

namespace Application.Features.Oeuvres.Commands.UpdateOeuvre;

// Retourne null si l'oeuvre est absente ou supprimée
public sealed record UpdateOeuvreCommand(
    string Id,
    string Titre,
    string? Resume,
    string? CouverturePremiere,
    string? CouvertureQuatrieme) : IRequest<OeuvreDto?>;
