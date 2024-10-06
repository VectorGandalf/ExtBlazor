namespace ExtBlazor.RemoteMediator;
public interface IRemoteMediator
{
    Task<TResult?> Send<TResult>(IRemoteRequest<TResult> request, CancellationToken ct = default);
    Task Send<TRequest>(TRequest request, CancellationToken ct = default) where TRequest : IRemoteRequest;
}
