using Microsoft.AspNetCore.Mvc;

namespace ExtBlazor.RemoteMediator.Server;
public class HttpRemoteMediatorServer
{
    public static async Task<TransportResponse> Handle([FromBody] TransportRequest transportRequest,
        HttpRemoteMediatorServerConfig config,
        IServiceProvider serviceProvider,
        CancellationToken ct)
    {
        var request = transportRequest.Request.ToObject();

        _ = config.MediatorCallback ?? throw new NoMediatorCallbackException();
        object? error = null;
        object? result = null;
        try
        {
            var processedRequest = config.RequestProcessor(request);
            result = await MediatorInvoker.Invoke(processedRequest, config.MediatorCallback, serviceProvider, ct);
            if (result is not null)
            {
                result = config.RequestProcessor(result);
            }
        }
        catch (Exception exception)
        {
            error = config.ExceptionHandler(exception);
        }

        if (error is not null)
        {
            return new(null, new(error));
        }
        else if (result is not null)
        {
            return new(new(result), null);
        }
        else
        {
            return new(null, null);
        }
    }
}
