using ExtRemoting.Types;
using System.Net.Http.Json;

namespace ExtRemoting.HttpTransport;
public class ExtRemotingService(IHttpClientFactory factory, IHttpTransportConfiguration configuration)
{
    public async Task<T> Send<T>(IRequest<T> request, CancellationToken cancellationToken = default)
    {
        var client = factory.CreateClient(configuration.HttpClientName);
        var httpResponse = await client.PostAsJsonAsync(configuration.EndpointUri, new Envelope(request), cancellationToken);
        if (httpResponse == null)
        {
            throw new Exception();
        }
        else
        {
            var response = await httpResponse.Content.ReadFromJsonAsync<IResponse>();
            if (response) 
            {
                response.
            }
            if (response!.Envelope != null) 
            {
                
            }
        }
    }
}
