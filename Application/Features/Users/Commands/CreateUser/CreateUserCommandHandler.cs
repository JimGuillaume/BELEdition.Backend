using Application.Common.Interfaces;
using Application.Common.Messaging;
using Application.Features.Users.Commands.UpdateUser;
using Application.Mappers;
using Application.Models;
using Domain.Entities;

namespace Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<CreateUserCommand, UserDto?>
{
    public async Task<UserDto?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.EmailExistsAsync(request.Email, cancellationToken))
            return null;

        User user;
        if (request.MotDePasseHash is not null)
        {
            user = User.CreerAvecMotDePasse(request.Nom, request.Prenom, request.Email, request.MotDePasseHash);
            if (request.EidIdentifiantHash is not null)
                user.LierItsme(request.EidIdentifiantHash);
        }
        else if (request.EidIdentifiantHash is not null)
        {
            user = User.CreerAvecItsme(request.Nom, request.Prenom, request.Email, request.EidIdentifiantHash);
        }
        else
        {
            throw new ArgumentException("Un mot de passe ou un identifiant itsme est obligatoire.");
        }

        await userRepository.AddAsync(user, cancellationToken);

        return user.ToDto();
    }
}