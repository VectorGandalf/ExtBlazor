using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExtBlazor.Core;

public static class IQueryableExtensions
{
    public static async Task<PagedSet<T>> PageAsync<T>(this IQueryable<T> query,
        IPagingationParameters pagingation)
    {
        return await query.PageAsync(
            pagingation.SortExp,
            pagingation.Asc,
            pagingation.Skip,
            pagingation.Take);
    }

    public static async Task<PagedSet<T>> PageAsync<T>(this IQueryable<T> query,
        string? sortProperty,
        bool? ascending,
        int? skip,
        int? take)
    {
        var items = (take != null && skip != null)
            ? await query.OrderBy(sortProperty, ascending).Skip((int)skip).Take((int)take).ToListAsync()
            : await query.OrderBy(sortProperty, ascending).ToListAsync();

        var totalCount = await query.CountAsync();

        return new(items, totalCount);
    }
}
