using Application.Common.Messaging;
using Application.Models;
using Domain.Entities;

namespace Application.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(string Id, string Nom, string Prenom, string Email, string? MotDePasseHash, bool? EstActif, bool? EstSuspendu) : IRequest<UserDto?>;