﻿using Microsoft.AspNetCore.Components;

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

    public string? SortExpression { get; private set; }

    private List<ColumnBase<TItem>> Columns { get; set; } = [];

    internal void AddColumn(ColumnBase<TItem> column)
    {
        Columns.Add(column);
        StateHasChanged();
    }

    internal async Task SignalColumnEvent(IColumnEventArgs args) 
    {
        if (args is SortEventData sortArgs) 
        {
            SortExpression = sortArgs.SortExpression;
            StateHasChanged();
        }

        await OnColumnEvent.InvokeAsync(args);
    }
}