using System.Net.Http.Json;

namespace ExtBlazor.RemoteMediator.Client;
public class HttpRemoteMediator(IHttpClientFactory httpClientFactory, HttpRemoteMediatorConfig settings) :
    IRemoteMediator
{
    public async Task<TResult?> Send<TResult>(IRemoteRequest<TResult> request, CancellationToken ct = default)
    {
        return (TResult?)(await SendHttp(request, ct));
    }

    public async Task Send(IRemoteRequest request, CancellationToken ct = default)
    {
        _ = await SendHttp(request, ct);
    }

    public async Task<object?> SendHttp(object remoteRequest, CancellationToken ct = default)
    {
        var payload = new JsonParcel(remoteRequest);
        var transportRequest = new TransportRequest(payload);
        var processedRequest = settings.RequestProcessor((IBaseRemoteRequest)remoteRequest);

        var client = httpClientFactory.CreateClient(settings.HttpClientName);
        var response = await client.PostAsJsonAsync<TransportRequest>(settings.EndPointUrl, transportRequest, ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Remote Mediator http request returned status code: " + response.StatusCode);
        }

        var transportResult = await response.Content.ReadFromJsonAsync<TransportResponse>();
        if (transportResult?.ErrorResponse is null)
        {
            return transportResult?.Response?.ToObject();
        }

        // Handle Error
        var error = transportResult?.ErrorResponse?.ToObject();
        settings.ErrorHandler(error);
        throw new Exception("Remote Mediator: Error handler did not throw an exception");
    }
}
