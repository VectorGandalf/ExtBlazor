namespace ExtRemoting.HttpTransport;

public class HttpTransportConfiguration : IHttpTransportConfiguration
{
    public required string HttpClientName { get; init; }
    public required string? EndpointUri { get; init; }
}