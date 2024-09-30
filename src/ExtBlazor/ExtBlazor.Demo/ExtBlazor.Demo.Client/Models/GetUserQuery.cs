using ExtBlazor.Core;

namespace ExtBlazor.Demo.Client.Models;

public class GetUserQuery : IQuery<UserDto>
{
    public int Id { get; set; }
}
