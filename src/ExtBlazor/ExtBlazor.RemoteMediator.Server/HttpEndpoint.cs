namespace ExtBlazor.RemoteMediator.Server;
public static class HttpEndpoint
{
    public static RouteHandlerBuilder MapRemoteMediatorEndPoint(this WebApplication app, string path = Constants.DEFAULT_ENDPOINT_URL)
    {
        return app.MapPost(path, HttpRemoteMediatorServer.Handle);
    }

    public static RouteHandlerBuilder MapRemoteMediatorEndPoint(this RouteGroupBuilder builder, string path = Constants.DEFAULT_ENDPOINT_URL)
    {
        return builder.MapPost(path, HttpRemoteMediatorServer.Handle);
    }
}
