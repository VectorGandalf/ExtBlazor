using ExtBlazor.Core;
using ExtBlazor.RemoteMediator;
using MediatR;

namespace ExtBlazor.Demo.Client.Models;
public interface IPageQuery<TItem> :
    IRemoteRequest<Page<TItem>>,
    IRequest<Page<TItem>>,
    IPageParameters
{
}
