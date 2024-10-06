using ExtBlazor.Core;

namespace ExtBlazor.Demo.Client.Models;

public class GetUserDtosQuery : IDemoPageQuery<UserDto>
{
    public string? Search { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public IEnumerable<SortExpression>? Sort { get; set; } = [
        new(nameof(UserDto.Admin), false),
        new(nameof(UserDto.Created), false),
        new(nameof(UserDto.Name), true)];
}
