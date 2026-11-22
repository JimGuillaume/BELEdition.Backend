using Application.Common.Messaging;

namespace Application.Features.Oeuvres.Commands.DeleteOeuvre;

// Soft delete ; retourne false si l'oeuvre est absente ou déjà supprimée
public sealed record DeleteOeuvreCommand(string Id) : IRequest<bool>;
