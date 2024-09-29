namespace ExtBlazor.Core;
public interface IPageQuery<TItem> : 
    IQuery<Page<TItem>>, 
    IPageParameters
{
}
