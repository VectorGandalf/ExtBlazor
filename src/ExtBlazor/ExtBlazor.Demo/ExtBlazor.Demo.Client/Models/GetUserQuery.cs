namespace ExtBlazor.Demo.Client.Models;

public class GetUserQuery : IDemoRequest<UserDto?>
{
    public int Id { get; set; }
}
