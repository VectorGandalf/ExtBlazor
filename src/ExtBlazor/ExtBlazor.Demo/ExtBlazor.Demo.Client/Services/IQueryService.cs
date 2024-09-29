using ExtBlazor.Core;

namespace ExtBlazor.Demo.Client.Services;

public interface IQueryService
{
    Task<TResult?> Query<TResult>(IQuery<TResult> query);
}