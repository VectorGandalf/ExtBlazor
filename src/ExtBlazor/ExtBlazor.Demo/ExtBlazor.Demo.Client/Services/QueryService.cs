using ExtBlazor.Core;
using ExtBlazor.Demo.Client.Models;
using System.Net.Http.Json;

namespace ExtBlazor.Demo.Client.Services;

public class QueryService(HttpClient httpClient) : IQueryService
{
    private Dictionary<Type, string> queryUrls = new()
    {
        { typeof(GetUsersQuery), "api/users" },
        { typeof(GetUserDtosQuery), "api/usersdtos" }
    };

    public async Task<TResult?> Query<TResult>(IQuery<TResult> query) 
    {
        var fullQueryUrl = queryUrls[query.GetType()] + query.ToQueryString();
        return await httpClient.GetFromJsonAsync<TResult>(fullQueryUrl);
    }
}
