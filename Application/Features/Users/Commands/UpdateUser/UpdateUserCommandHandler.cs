using Application.Common.Interfaces;
using Application.Common.Messaging;
using Application.Mappers;
using Application.Models;

namespace Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<UpdateUserCommand, UserDto?>
{
    public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
            return null;

        user.ModifierProfil(request.Nom, request.Prenom, request.Email, request.MotDePasseHash, request.EstActif, request.EstSuspendu);

        await userRepository.UpdateAsync(user, cancellationToken);

        return user.ToDto();
    }
}
