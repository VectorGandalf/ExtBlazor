using ExtBlazor.Core;

namespace ExtBlazor.Demo.Client.Models;

public class GetUserDtosQuery : IPageQuery<UserDto>
{
    public string? Search { get; set; }
    public string? Sort { get; set; } = $"{nameof(UserDto.Name)}";
    public int? Skip { get; set; }
    public int? Take { get; set; }
}
