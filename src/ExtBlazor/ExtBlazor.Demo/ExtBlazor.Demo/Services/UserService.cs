using ExtBlazor.Core;
using ExtBlazor.Demo.Client.Models;
using ExtBlazor.Demo.Database;
using Microsoft.EntityFrameworkCore;

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
                user.Name.ToLower().Contains(token.ToLower()) ||
                user.Phone.ToLower().Contains(token.ToLower()) ||
                user.Email.ToLower().Contains(token.ToLower()) ||
                user.Username.ToLower().Contains(token.ToLower()));
        }

        return await queryable.ToPagedSetAsync(query);
    }
}
