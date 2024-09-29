using Microsoft.AspNetCore.Components;

namespace ExtBlazor.Grid;
[CascadingTypeParameter(nameof(TItem))]
public partial class Grid<TItem>
{
    [Parameter]
    public IEnumerable<TItem> Items { get; set; } = [];

    [Parameter]
    public EventCallback<IColumnEventArgs> OnColumnEvent { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? CssClass { get; set; }

    [Parameter]
    public Type DefaultHeadTempate { get; set; } = typeof(DefaultHeadTemplate<TItem>);

    public string? SortExpression { get; private set; }

    private List<ColumnBase<TItem>> Columns { get; set; } = [];

    internal void AddColumn(ColumnBase<TItem> column)
    {
        Columns.Add(column);
        StateHasChanged();
    }

    internal async Task SignalColumnEvent(IColumnEventArgs args) 
    {
        if (args is ColumnSortEventArgs sortArgs) 
        {
            SortExpression = sortArgs.SortExpression;
            StateHasChanged();
        }

        await OnColumnEvent.InvokeAsync(args);
    }
}
