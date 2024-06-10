using ExtBlazor.Core;
using ExtBlazor.Demo.Client.Models;
using ExtBlazor.Demo.Database;

namespace ExtBlazor.Demo.Services;

public interface IUserService
{
    Task<PagedSet<User>> GetUsers(GetUsersQuery query);
}

public class UserService(ExDbContext dbContext) : IUserService
{
    public async Task<PagedSet<User>> GetUsers(GetUsersQuery query)
    {
        var queryable = dbContext.Users.AsQueryable();
        var tokens = query.Search?.Split(' ') ?? [];

        foreach (var token in tokens)
        {
            queryable = queryable.Where(user =>
                user.Name.Contains(token) ||
                user.Phone.Contains(token) ||
                user.Email.Contains(token) ||
                user.Username.Contains(token));
        }

        return await queryable.ToPagedSetAsync(query);
    }
}
