using System.Linq.Expressions;

namespace ExtBlazor.Core;

public static class IQueryableExtensions
{
    public static PagedSet<T> Page<T>(this IQueryable<T> query,
        IPagingationParameters pagingation)
    {
        return query.Page(
            pagingation.SortExp,
            pagingation.Asc,
            pagingation.Skip,
            pagingation.Take);
    }

    public static PagedSet<T> Page<T>(this IQueryable<T> query,
        string? sortProperty,
        bool? ascending,
        int? skip,
        int? take)
    {
        var items = (take != null && skip != null)            
            ? query.OrderBy(sortProperty, ascending).Skip((int)skip).Take((int)take).ToList()
            : query.OrderBy(sortProperty, ascending).ToList();

        var totalCount = query.Count();

        return new(items, totalCount);
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string? sortProperty, bool? ascending)
    {
        if (sortProperty is null || ascending is null)
        {
            return queryable;
        }

        var parameterExpression = Expression.Parameter(typeof(T), "p");
        var memberExpression = Expression.Property(parameterExpression, sortProperty);
        var lambdaExpression = Expression.Lambda(memberExpression, parameterExpression);

        var method = (bool)ascending
            ? "OrderBy"
            : "OrderByDescending";

        var types = new Type[]
        {
            queryable.ElementType,
            lambdaExpression.Body.Type
        };

        var expression = Expression.Call(
            typeof(Queryable),
            method, types,
            queryable.Expression,
            lambdaExpression);

        return queryable.Provider.CreateQuery<T>(expression);
    }
}
