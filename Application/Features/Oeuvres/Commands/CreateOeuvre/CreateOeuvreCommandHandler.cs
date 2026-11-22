using Application.Common.Interfaces;
using Application.Common.Messaging;
using Application.Mappers;
using Application.Models;
using Domain.Entities;

namespace Application.Features.Oeuvres.Commands.CreateOeuvre;

public sealed class CreateOeuvreCommandHandler(IOeuvreRepository oeuvreRepository)
    : IRequestHandler<CreateOeuvreCommand, OeuvreDto>
{
    public async Task<OeuvreDto> Handle(CreateOeuvreCommand request, CancellationToken cancellationToken)
    {
        var oeuvre = new Oeuvre
        {
            Titre = request.Titre,
            Resume = request.Resume,
            CouverturePremiere = request.CouverturePremiere,
            CouvertureQuatrieme = request.CouvertureQuatrieme,
            IdAuteur = request.IdAuteur,
            IdType = request.IdType,
            IdStatus = request.IdStatus,
            DateCreation = DateTime.UtcNow,
        };

        await oeuvreRepository.AddAsync(oeuvre, cancellationToken);

        return oeuvre.ToDto();
    }
}
