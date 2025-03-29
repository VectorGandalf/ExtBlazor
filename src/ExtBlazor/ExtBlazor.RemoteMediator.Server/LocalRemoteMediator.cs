namespace ExtBlazor.RemoteMediator.Server;

public class LocalRemoteMediator(
    HttpRemoteMediatorServerConfig config,
    IServiceScopeFactory serviceScopeFactory) : IRemoteMediator
{
    public async Task<TResult?> Send<TResult>(IRemoteRequest<TResult> request, CancellationToken ct = default)
    {
        _ = config.MediatorCallback ?? throw new NoMediatorCallbackException();
        var processedRequest = config.RequestProcessor(request);
        var result = (TResult?)await MediatorInvoker.Invoke(
            processedRequest,
            config.MediatorCallback,
            serviceScopeFactory,
            ct);
        if (result != null)
        {
            return (TResult?)config.RequestProcessor(result);
        }

        return result;
    }

    public async Task Send(IRemoteRequest request, CancellationToken ct = default)
    {
        _ = config.MediatorCallback ?? throw new NoMediatorCallbackException();
        var processedRequest = config.RequestProcessor(request);
        await MediatorInvoker.Invoke(
            processedRequest,
            config.MediatorCallback,
            serviceScopeFactory,
            ct);
    }
}
