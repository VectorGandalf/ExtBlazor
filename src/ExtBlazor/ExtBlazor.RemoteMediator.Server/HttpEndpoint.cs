namespace ExtBlazor.RemoteMediator.Server;
public static class HttpEndpoint
{
    public static RouteHandlerBuilder MapRemoteMediatorEndPoint(this IEndpointRouteBuilder builder, string path = Constants.DEFAULT_ENDPOINT_URL)
    {
        return builder.MapPost(path, HttpRemoteMediatorServer.Handle);
    }
}
