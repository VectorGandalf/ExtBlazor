using ExtBlazor.Core;
using ExtBlazor.Core.Server;
using ExtBlazor.Demo.Client.Models;
using ExtBlazor.Demo.Database;

namespace ExtBlazor.Demo.Services;

public interface IUserService
{
    Task<Page<User>> GetUsers(GetUsersQuery query);
    Task<Page<UserDto>> GetUserDtos(GetUserDtosQuery query);
}

public class UserService(ExDbContext dbContext) : IUserService
{
    public async Task<Page<User>> GetUsers(GetUsersQuery query)
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

        return await queryable.PageAsync(query);
    }

    public async Task<Page<UserDto>> GetUserDtos(GetUserDtosQuery query)
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
        var queryableDtos = queryable.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Username = u.Username,
            ContactInformation = new() { Email = u.Email, Phone = u.Phone },
            Changed = u.Changed,
            Created = u.Created,
            LastLogin = u.LastLogin
        });

        try
        {
            var page = await queryableDtos.PageAsync(query);
            return page;
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }
}
