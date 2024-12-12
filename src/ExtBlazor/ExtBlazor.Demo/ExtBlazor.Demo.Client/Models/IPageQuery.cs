using ExtBlazor.Core;

namespace ExtBlazor.Demo.Client.Models;
public interface IPageQuery<TItem> :
    IDemoRequest<Page<TItem>>,
    IPageParameters;
