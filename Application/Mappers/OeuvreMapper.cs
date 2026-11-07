using Application.Models;
using Domain.Entities;

namespace Application.Mappers;

public static class OeuvreMapper
{
    public static OeuvreDto ToDto(this Oeuvre oeuvre) => new(
        oeuvre.Id,
        oeuvre.Titre,
        oeuvre.Resume,
        oeuvre.CouverturePremiere,
        oeuvre.CouvertureQuatrieme,
        oeuvre.DateCreation,
        oeuvre.DateModification);

    public static IEnumerable<OeuvreDto> ToDto(this IEnumerable<Oeuvre> oeuvres)
        => oeuvres.Select(ToDto);
}
