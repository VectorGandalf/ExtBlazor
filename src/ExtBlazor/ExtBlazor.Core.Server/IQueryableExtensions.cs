using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using ExtBlazor.Core;

namespace ExtBlazor.Core.Server;

public static class IQueryableExtensions
{
    public static async Task<Page<T>> PageAsync<T>(this IQueryable<T> query,
        IPageParameters pagingation,
        CancellationToken ct = default)
     => await query.PageAsync(
            pagingation.Sort,
            pagingation.Skip,
            pagingation.Take,
            ct);


    public static async Task<Page<T>> PageAsync<T>(this IQueryable<T> query,
        IEnumerable<SortExpression>? sortPropertyExpressions,
        int? skip,
        int? take,
        CancellationToken ct = default)
    {
        query = query.Sort((sortPropertyExpressions ?? []).ToArray());
        var items = (take != null && skip != null)
            ? await query.Skip((int)skip).Take((int)take).ToListAsync(ct)
            : await query.ToListAsync(ct);

        var totalCount = await query.CountAsync(ct);

        return new(items, totalCount);
    }
}
