namespace ExtBlazor.Core;
public class Pagination
{
    public int Take { get; set; }
    public int Skip { get; set; }
    public int TotalCount { get; set; }

    public int Pages => Take > 0 
        ? (int)Math.Ceiling((decimal)TotalCount / (decimal)Take) 
        : 0;

    public int CurrentPage => Skip != 0 
        ? (int)Math.Ceiling((decimal)Skip / (decimal)Take) + 1 
        : 1;

    public bool HasNext => Pages - CurrentPage > 0;

    public bool HasPrevious => CurrentPage > 1;

    public void SetTotalNumberOfItem(int value)
    {
        if (value != TotalCount)
        {
            TotalCount = value;
            SetPage(1);
        }
    }

    public void SetTake(int value)
    {
        Take = value;
        SetPage(1);
    }

    public void SetSkip(int value)
    {
        Skip = value;
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
            pageNumber = Pages;
        }

        Skip = (pageNumber - 1) * Take;
        OnCurrentPageChanged?.Invoke(new(), new());
    }

    public event EventHandler? OnCurrentPageChanged;
}
