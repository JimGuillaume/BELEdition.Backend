using Application.Models;
using Domain.Entities;

namespace Application.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(this User user) => new(
        user.Id,
        user.Nom,
        user.Prenom,
        user.Email,
        //user.MotDePasseHash,
        //user.EidIdentifiantHash,
        user.EstActif,
        user.EstSuspendu,
        user.TentativesEchouees,
        user.VerrouilleJusqua,
        user.DateCreation,
        user.DateModification);

    public static IEnumerable<UserDto> ToDto(this IEnumerable<User> users)
        => users.Select(ToDto);
}
