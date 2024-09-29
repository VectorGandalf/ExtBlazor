namespace ExtBlazor.Core;
public static class IQueryExtensions
{
    public static string ToQueryString<TResult>(this IQuery<TResult> query) 
    {
        Dictionary<string, string?> pairs = new();
                
        foreach (var prop in query.GetType().GetProperties()) 
        {
            pairs.Add(prop.Name, prop.GetValue(query)?.ToString());
        }

        var notNull = pairs.Where(_ => _.Value != null);

        return notNull.Any()
            ? "?" + string.Join("&", notNull.Select(_ => _.Key + "=" + _.Value))
            : string.Empty;
    }
}
