using ExtBlazor.Core;
using ExtBlazor.Demo.Client.Models;
using System.Net.Http.Json;
using System.Reflection;

namespace ExtBlazor.Demo.Client.Services;

public class QueryService(HttpClient httpClient) : IQueryService
{
    private Dictionary<Type, string> queryUrls = new()
    {
        { typeof(GetUsersQuery), "api/users" },
        { typeof(GetUserDtosQuery), "api/usersdtos" },
        { typeof(GetUserQuery), "api/users/{id}" }
    };

    public async Task<TResult?> Query<TResult>(IQuery<TResult> query)
    {
        var urlTemplete = queryUrls[query.GetType()];
        var url = ToUrl(urlTemplete, query);
        return await httpClient.GetFromJsonAsync<TResult>(url);
    }

    private string ToUrl<TResult>(string urlTemplate, IQuery<TResult> query)
    {
        Dictionary<string, string> queryStringDict = new();
        Dictionary<string, string> pathDict = new();

        foreach (var prop in query.GetType().GetProperties())
        {
            var value = prop.GetValue(query);
            if (value != null)
            {
                if (value is IEnumerable<object> enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        if (item != null)
                        {
                            queryStringDict.Add(prop.Name, item.ToString() ?? "");
                        }
                    }
                }
                else
                {
                    var token = ToPathToken(prop);
                    if (urlTemplate.ToLowerInvariant().IndexOf(token) > -1)
                    {
                        pathDict.Add(token, value.ToString() ?? "");
                    }
                    queryStringDict.Add(prop.Name, value.ToString() ?? "");
                }
            }
        }

        var path = urlTemplate;
        foreach (var pair in pathDict)
        {
            path = path.Replace(pair.Key, pair.Value);
        }

        var queryString = queryStringDict.Any()
            ? "?" + string.Join("&", queryStringDict.Select(_ => _.Key + "=" + _.Value))
            : string.Empty;

        var url = path + queryString;
        return url;
    }

    private static string ToPathToken(PropertyInfo prop)
    {
        return "{" + prop.Name.ToLowerInvariant() + "}";
    }
}
