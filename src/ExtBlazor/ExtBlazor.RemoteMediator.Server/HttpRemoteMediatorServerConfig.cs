
namespace ExtBlazor.RemoteMediator.Server;

public class HttpRemoteMediatorServerConfig
{
    public Delegate? MediatorCallback { get; set; }
    public Func<Exception, object> ExceptionHandler { get; set; } = exception => exception;
    public Func<object, object> RequestProcessor { get; set; } = request => request;
    public Func<object, object> ResponseProcessor { get; set; } = response => response;
}