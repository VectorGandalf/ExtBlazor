namespace ExtBlazor.Demo.Client.Models;

public record UserDto
{
    public int Id { get; init; }
    public required string Username { get; init; }
    public required string Name { get; init; }
    public required ContactInformationDto ContactInformation { get; init; }
    public DateTime Created { get; init; }
    public DateTime Changed { get; init; }
    public DateTime? LastLogin { get; init; }
}

public record ContactInformationDto
{
    public required string Email { get; init; }
    public required string Phone { get; init; }
}