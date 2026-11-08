using Application.Common.Interfaces;
using Application.Common.Messaging;
using Application.Mappers;
using Application.Models;

namespace Application.Features.Users.Queries.GetUser;

public sealed class GetAllActiveUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllActiveUsersQuery, UserDto[]>
{
    public async Task<UserDto[]> Handle(GetAllActiveUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllActiveAsync(cancellationToken);

        return users.ToDto().ToArray();
    }
}