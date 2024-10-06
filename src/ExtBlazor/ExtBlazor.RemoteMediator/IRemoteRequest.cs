namespace ExtBlazor.RemoteMediator;
public interface IBaseRemoteRequest;
public interface IRemoteRequest : IBaseRemoteRequest;
public interface IRemoteRequest<out TResult> : IBaseRemoteRequest;