
using ExtRemoting.Types;
using System.Net.Http;

namespace ExtRemoting.HttpClient;
public class ExtRemotingService(IHttpClientFactory factory)
{
    public Task<T> Send<T>(IRequest<T> request) 
    {
        return new Task<T>();
    }
}
