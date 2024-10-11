using ExtBlazor.Core;

namespace ExtBlazor.Demo.Client.Models;

public class GetUsersQuery : IPageQuery<User>
{
    public string? Search { get; set; }
    public IEnumerable<SortExpression>? Sort { get; set; } = [new(nameof(User.Name))];
    public int? Skip { get; set; }
    public int? Take { get; set; }
}
