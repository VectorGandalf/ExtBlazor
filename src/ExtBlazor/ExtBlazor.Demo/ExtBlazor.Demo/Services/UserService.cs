using ExtBlazor.Core;
using ExtBlazor.Core.Server;
using ExtBlazor.Demo.Client.Models;
using ExtBlazor.Demo.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExtBlazor.Demo.Services;

public interface IUserService
{
    Task<Page<User>> GetUsers(GetUsersQuery query, CancellationToken ct = default);
    Task<Page<UserDto>> GetUserDtos(GetUserDtosQuery query, CancellationToken ct = default);
    Task<UserDto?> GetUser(int id, CancellationToken ct = default);
}

public class UserService(ExDbContext dbContext) : IUserService
{
    public async Task<Page<User>> GetUsers(GetUsersQuery query, CancellationToken ct = default)
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

        return await queryable.PageAsync(query, ct);
    }

    public async Task<Page<UserDto>> GetUserDtos(GetUserDtosQuery query, CancellationToken ct = default)
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
        var queryableDtos = queryable.Select(UserProjection);

        try
        {
            var page = await queryableDtos.PageAsync(query, ct);
            return page;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }   

    public async Task<UserDto?> GetUser(int id, CancellationToken ct = default)
    {
        return await dbContext.Users
            .Where(u => u.Id == id)
            .Select(UserProjection)
            .FirstOrDefaultAsync(ct);
    }

    private static Expression<Func<User, UserDto>> UserProjection = u => new UserDto
    {
        Id = u.Id,
        Name = u.Name,
        Username = u.Username,
        ContactInformation = new() { Email = u.Email, Phone = u.Phone },
        Changed = u.Changed,
        Created = u.Created,
        LastLogin = u.LastLogin,
        Admin = u.Admin
    };
}
