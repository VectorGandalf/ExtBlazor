using ExtBlazor.Core;

namespace ExtBlazor.Tests.Units.Paging;

public class PaginationTests
{
    [Theory]
    [InlineData(11, 10, 2)]
    [InlineData(10, 10, 1)]
    [InlineData(1, 100, 1)]
    [InlineData(110, 20, 6)]
    [InlineData(121, 20, 7)]
    [InlineData(123, 20, 7)]
    [InlineData(107, 20, 6)]
    [InlineData(99, 20, 5)]
    public void PagesTest(int total, int take, int expectedPages)
    {
        var nav = new Pagination();
        nav.SetTotalNumberOfItem(total);
        nav.SetTake(take);
        Assert.Equal(expectedPages, nav.Pages);
    }

    [Theory]
    [InlineData(11, 10, 0, 1)]
    [InlineData(11, 10, 10, 2)]
    [InlineData(30, 10, 20, 3)]
    [InlineData(30, 20, 21, 3)]
    [InlineData(30, 3, 9, 4)]
    [InlineData(30, 3, 6, 3)]
    public void CurrentPageTest(int total, int take, int skip, int expectedPage)
    {
        Pagination nav = new();
        nav.SetTotalNumberOfItem(total);
        nav.SetTake(take);
        nav.SetSkip(skip);
        Assert.Equal(expectedPage, nav.CurrentPage);
    }

    [Theory]
    [InlineData(0, 3, 1, 7)]
    [InlineData(20, 3, 1, 7)]
    [InlineData(30, 3, 1, 7)]
    [InlineData(40, 3, 2, 8)]
    [InlineData(990, 3, 94, 100)]
    [InlineData(990, 5, 90, 100)]
    [InlineData(980, 5, 90, 100)]
    [InlineData(970, 5, 90, 100)]
    [InlineData(960, 5, 90, 100)]
    [InlineData(950, 5, 90, 100)]
    [InlineData(940, 5, 90, 100)]
    [InlineData(930, 5, 89, 99)]
    public void GetWindowTest(int skip, int padding, int expectedFirst, int expectedLast)
    {
        Pagination nav = new();
        nav.SetTotalNumberOfItem(1000);
        nav.SetTake(10);
        nav.SetSkip(skip);

        var window = nav.GetWindow(padding);

        Assert.Equal(expectedFirst, window.First());
        Assert.Equal(expectedLast, window.Last());
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(-1, 1)]
    [InlineData(int.MinValue, 1)]
    [InlineData(101, 100)]
    [InlineData(2000, 100)]
    [InlineData(int.MaxValue, 100)]
    public void SetPageTest(int pageNumber, int expectedCurrentPage)
    {
        Pagination nav = new();
        nav.SetTotalNumberOfItem(1000);
        nav.SetTake(10);
        nav.SetSkip(0);

        nav.SetPage(pageNumber);

        Assert.Equal(expectedCurrentPage, nav.CurrentPage);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void PreviousTest(int pageNumber)
    {
        var nav = new Pagination();
        nav.SetTotalNumberOfItem(1000);
        nav.SetTake(10);
        nav.SetSkip(0);

        nav.SetPage(pageNumber);

        nav.Next();

        Assert.Equal(pageNumber + 1, nav.CurrentPage);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    public void NextTest(int pageNumber)
    {
        var nav = new Pagination();
        nav.SetTotalNumberOfItem(1000);
        nav.SetTake(10);
        nav.SetSkip(0);

        nav.SetPage(pageNumber);

        nav.Previous();

        Assert.Equal(pageNumber - 1, nav.CurrentPage);
    }
}
