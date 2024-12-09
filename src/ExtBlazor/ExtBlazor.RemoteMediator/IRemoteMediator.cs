namespace ExtBlazor.RemoteMediator;
public interface IRemoteMediator
{
    Task<TResult?> Send<TResult>(IRemoteRequest<TResult> request, CancellationToken ct = default);
    Task Send(IRemoteRequest request, CancellationToken ct = default);
}
