namespace ExtRemoting.HttpTransport;

public interface IHttpTransportConfiguration
{
    string HttpClientName { get; init; }
    string EndpointUri { get; set; }
}