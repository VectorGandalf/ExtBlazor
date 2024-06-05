﻿using Microsoft.AspNetCore.Components;

namespace ExtBlazor.Grid;

public abstract class ColumnBase<TItem> : ComponentBase
{
    [CascadingParameter(Name = "ParentGrid")]
    protected Grid<TItem>? Grid { get; set; }

    [Parameter]
    public string? Title { get; set; }
    [Parameter]
    public bool Sortable { get; set; }
    [Parameter]
    public string? SortColumn { get; set; }
    [Parameter]
    public bool DefaultSortColumn { get; set; }
    [Parameter]
    public bool DefaultSortDirectionAsc { get; set; }
    [Parameter]
    public virtual RenderFragment<ColumnBase<TItem>>? HeadTemplate { get; set; }
    [Parameter]
    public virtual RenderFragment<ColumnBase<TItem>>? FootTemplate { get; set; }
    protected override void OnInitialized()
    {
        if (Grid != null)
        {
            Grid.AddColumn(this);
        }

        base.OnInitialized();
    }

    internal virtual Task OnColumnEventHandler(IColumnEventArgs args) 
    {
        return Task.CompletedTask;
    }

    internal abstract string? GetValue(TItem item);
}