using System.Linq.Expressions;

namespace ExtBlazor.Core;

public static class IQueryableExtensions
{
    public static Page<T> Page<T>(this IQueryable<T> query,
        IPageParameters pagingation)
    {
        return query.Page(
            pagingation.Sort,
            pagingation.Skip,
            pagingation.Take);
    }

    public static Page<T> Page<T>(this IQueryable<T> query,
        string? sortPropertiesExpression,
        int? skip,
        int? take)
    {
        query = query.Sort(sortPropertiesExpression);

        var items = (take != null && skip != null)
            ? query.Skip((int)skip).Take((int)take).ToList()
            : query.ToList();

        var totalCount = query.Count();

        return new(items, totalCount);
    }

    public static IQueryable<T> Sort<T>(this IQueryable<T> query,
        string? sort)
    {
        var sortPropertyExpressions = Parse(sort);
        for (int i = 0; i < sortPropertyExpressions.Length; i++)
        {
            var (property, ascending) = sortPropertyExpressions[i];

            var isFirst = i == 0;
            var method = ParseMethod(ascending, isFirst);

            query = query.OrderBy(method, property);
        }

        return query;
    }

    private static string ParseMethod(bool ascending, bool isFirst) => ascending
        ? isFirst ? "OrderBy" : "ThenBy"
        : isFirst ? "OrderByDescending" : "ThenByDescending";

    private static (string, bool)[] Parse(string? sortPropertiesExpression) => (sortPropertiesExpression ?? "")
        .Split(',')
        .Select(s => s.Split(' '))
        .Select(s => (s[0], s.Length < 2 || s[1] == "asc"))
        .ToArray();

    private static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> queryable,
        string method,
        string property)
    {
        var parameterExpression = Expression.Parameter(typeof(T), "p");
        var memberExpression = property.Split('.').Aggregate<string, Expression>(parameterExpression, Expression.PropertyOrField);
        var lambdaExpression = Expression.Lambda(memberExpression, parameterExpression);

        var types = new Type[]
        {
            queryable.ElementType,
            lambdaExpression.Body.Type
        };

        var expression = Expression.Call(
            typeof(Queryable),
            method,
            types,
            queryable.Expression,
            lambdaExpression);

        return (IOrderedQueryable<T>)queryable.Provider.CreateQuery<T>(expression);
    }
}
