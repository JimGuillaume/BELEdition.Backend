using Application.Common.Messaging;
using Application.Models;
using Domain.Entities;

namespace Application.Features.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(string Id) : IRequest<UserDto?>;