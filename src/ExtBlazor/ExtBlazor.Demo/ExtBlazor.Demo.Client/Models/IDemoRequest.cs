using ExtBlazor.RemoteMediator;
using MediatR;

namespace ExtBlazor.Demo.Client.Models;

public interface IDemoRequest<TResult> :
    IRemoteRequest<TResult>,
    IRequest<TResult>;

public interface IDemoRequest :
    IRemoteRequest,
    IRequest;