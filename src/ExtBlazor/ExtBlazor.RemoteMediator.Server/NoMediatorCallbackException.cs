
namespace ExtBlazor.RemoteMediator.Server;

[Serializable]
public class NoMediatorCallbackException : Exception
{
    public NoMediatorCallbackException() : this("You must configure HttpRemoteMediatorServerConfig.MediatorCallback")
    {
    }

    public NoMediatorCallbackException(string? message) : base(message)
    {
    }

    public NoMediatorCallbackException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}