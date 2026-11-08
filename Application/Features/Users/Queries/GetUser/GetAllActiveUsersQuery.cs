using Application.Common.Messaging;
using Application.Models;

namespace Application.Features.Users.Queries.GetUser;

public sealed record GetAllActiveUsersQuery() : IRequest<UserDto[]>;
