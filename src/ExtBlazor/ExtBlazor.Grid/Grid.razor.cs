using ExtBlazor.Core;
using Microsoft.AspNetCore.Components;

namespace ExtBlazor.Grid;
[CascadingTypeParameter(nameof(TItem))]
public partial class Grid<TItem> : ISortable
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
    public Type DefaultHeadTempate { get; set; } = typeof(DefaultHeadTemplate);

    [Parameter]
    public Func<TItem, string?> RowCssClass { get; set; } = item => null;

    [Parameter]
    public IEnumerable<SortExpression> Sort { get; set; } = [];

    [Parameter]
    public bool MultiColumnSort { get; set; } = false;

    private List<ColumnBase<TItem>> Columns { get; set; } = [];

    internal void AddColumn(ColumnBase<TItem> column)
    {
        Columns.Add(column);
        StateHasChanged();
    }

    public async Task Signal(IColumnEventArgs args)
    {
        if (args is ColumnSortEventArgs sortArgs)
        {
            Sort = sortArgs.SortExpressions;

            StateHasChanged();
        }

        await OnColumnEvent.InvokeAsync(args);
    }
}
