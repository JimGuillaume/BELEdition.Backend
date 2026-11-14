using Application.Common.Interfaces;
using Application.Common.Messaging;
using Application.Mappers;
using Application.Models;

namespace Application.Features.Users.Queries.GetUser;

public sealed class GetUserByEidHashQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserByEidHashQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByEidHashQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEidHashAsync(request.Eid, cancellationToken);

        return user?.ToDto();
    }
}