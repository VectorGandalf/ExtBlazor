namespace ExtBlazor.Demo.Client.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public DateTime Created { get; set; }
    public DateTime Changed { get; set; }
    public DateTime? LastLogin { get; set; }
}
