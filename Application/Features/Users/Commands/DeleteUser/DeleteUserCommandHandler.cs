using Application.Common.Interfaces;
using Application.Common.Messaging;
using Application.Mappers;
using Application.Models;

namespace Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<DeleteUserCommand, UserDto?>
{
    public async Task<UserDto?> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
            return null;

        user.Supprimer();

        await userRepository.UpdateAsync(user, cancellationToken);

        return user.ToDto();
    }
}
