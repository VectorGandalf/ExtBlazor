namespace ExtBlazor.Events;
public class EventHubPathConfiguration
{
    public required string Path { get; set; }
    public const string DEFAULT_PATH = "/eventhub";
}