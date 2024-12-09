using ExtBlazor.Core;
using ExtBlazor.Demo.Client.Models;
using ExtBlazor.Demo.Services;
using MediatR;

namespace ExtBlazor.Demo.Handlers;

public class UserHandlers(IUserService users) :
    IRequestHandler<GetUserDtosQuery, Page<UserDto>>,
    IRequestHandler<GetUsersQuery, Page<User>>,
    IRequestHandler<GetUserQuery, UserDto?>
{
    public async Task<Page<UserDto>> Handle(GetUserDtosQuery request, CancellationToken cancellationToken)
    {
        return await users.GetUserDtos(request, cancellationToken);
    }

    public async Task<Page<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await users.GetUsers(request, cancellationToken);
    }

    public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await users.GetUser(request.Id, cancellationToken);
    }
}
