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
        IEnumerable<SortExpression>? sortPropertiesExpression,
        int? skip,
        int? take)
    {
        query = query.Sort((sortPropertiesExpression ?? []).ToArray());

        var items = (take != null && skip != null)
            ? query.Skip((int)skip).Take((int)take).ToList()
            : query.ToList();

        var totalCount = query.Count();

        return new(items, totalCount);
    }

    public static IQueryable<T> Sort<T>(this IQueryable<T> query,
        SortExpression[] sort)
    {
        for (int i = 0; i < sort.Length; i++)
        {
            var sortExpression = sort[i];

            var isFirst = i == 0;
            var method = ParseMethod(sortExpression.Ascending ?? true, isFirst);

            query = query.OrderBy(method, sortExpression.Property);
        }

        return query;
    }

    private static string ParseMethod(bool ascending, bool isFirst) => ascending
        ? isFirst ? "OrderBy" : "ThenBy"
        : isFirst ? "OrderByDescending" : "ThenByDescending";

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
