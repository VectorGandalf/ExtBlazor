namespace ExtBlazor.Core;
public class Pagination
{
    public Action? OnNavigation;

    private int skip;
    private int take;
    private int totalCount;

    public int Take { get => take; set => SetTake(value); }
    public int Skip { get => skip; set => SetSkip(value); }
    public int TotalCount { get => totalCount; set => SetTotalCount(value); }

    public int Pages => Take > 0 
        ? (int)Math.Ceiling((decimal)TotalCount / (decimal)Take) 
        : 0;

    public int CurrentPage => Skip != 0 
        ? (int)Math.Ceiling((decimal)Skip / (decimal)Take) + 1 
        : 1;

    public bool HasNext => Pages - CurrentPage > 0;

    public bool HasPrevious => CurrentPage > 1;

    private void SetTotalCount(int value)
    {
        if (value != totalCount)
        {
            totalCount = value;
            SetPage(1);
        }
    }

    private void SetTake(int value)
    {
        take = value;
        SetPage(1);
    }

    private void SetSkip(int value)
    {
        skip = value < 0 
            ?  0 
            : value;
    }

    public IEnumerable<int> GetWindow(int paddingAroundCurrentPage)
    {
        var first = CurrentPage - paddingAroundCurrentPage;
        var last = CurrentPage + paddingAroundCurrentPage;

        if (first < 1)
        {
            last = (last + -first) + 1;
        }
        else if (last > Pages)
        {
            first = first - (last - Pages);
        }

        if (last > Pages)
        {
            last = Pages;
        }

        if (first < 1)
        {
            first = 1;
        }

        return Enumerable.Range(first, last - first + 1);
    }

    public void First() => SetPage(1);
    public void Last() => SetPage(Pages);
    public void Next() => SetPage(CurrentPage + 1);
    public void Previous() => SetPage(CurrentPage - 1);
    public void SetPage(int pageNumber)
    {
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }

        if (pageNumber >= Pages)
        {
            pageNumber = Pages > 0 
                ? Pages 
                : 1;
        }

        skip = (pageNumber - 1) * take;

        OnNavigation?.Invoke();
    }
}
