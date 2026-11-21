using Application.Common.Interfaces;
using Application.Common.Messaging;
using Application.Mappers;
using Application.Models;

namespace Application.Features.Oeuvres.Commands.UpdateOeuvre;

public sealed class UpdateOeuvreCommandHandler(IOeuvreRepository oeuvreRepository)
    : IRequestHandler<UpdateOeuvreCommand, OeuvreDto?>
{
    public async Task<OeuvreDto?> Handle(UpdateOeuvreCommand request, CancellationToken cancellationToken)
    {
        var oeuvre = await oeuvreRepository.GetByIdAsync(request.Id, cancellationToken);
        if (oeuvre is null)
        {
            return null;
        }

        oeuvre.Titre = request.Titre;
        oeuvre.Resume = request.Resume;
        oeuvre.CouverturePremiere = request.CouverturePremiere;
        oeuvre.CouvertureQuatrieme = request.CouvertureQuatrieme;
        oeuvre.DateModification = DateTime.UtcNow;

        // false si supprimée entre la lecture et l'écriture
        var updated = await oeuvreRepository.UpdateAsync(oeuvre, cancellationToken);

        return updated ? oeuvre.ToDto() : null;
    }
}
