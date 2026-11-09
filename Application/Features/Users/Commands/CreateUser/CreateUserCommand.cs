using Application.Common.Messaging;
using Application.Models;
using Domain.Entities;

namespace Application.Features.Users.Commands.UpdateUser;

public sealed record CreateUserCommand(
    string Nom, 
    string Prenom, 
    string Email, 
    string? MotDePasseHash,
    string? EidIdentifiantHash

    ) : IRequest<UserDto?>;