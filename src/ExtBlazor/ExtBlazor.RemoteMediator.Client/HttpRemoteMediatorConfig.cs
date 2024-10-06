namespace ExtBlazor.RemoteMediator.Client;

public class HttpRemoteMediatorConfig()
{
    public string HttpClientName { get; set; } = "Default";
    public Func<IBaseRemoteRequest, IBaseRemoteRequest> RequestProcessor { get; set; } = request => request;
    public Action<object?> ErrorHandler { get; set; } = error => throw new Exception("No Remote Mediator Error handler");
    public string EndPointUrl { get; set; } = Constants.DEFAULT_ENDPOINT_URL;
}