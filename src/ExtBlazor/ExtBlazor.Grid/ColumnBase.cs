using Microsoft.AspNetCore.Components;

namespace ExtBlazor.Grid;

public abstract class ColumnBase<TItem> : ComponentBase, IColumn
{
    [CascadingParameter(Name = "ParentGrid")]
    protected Grid<TItem>? Grid { get; set; }
    [Parameter]
    public virtual RenderFragment<TItem>? ChildContent { get; set; }
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public virtual RenderFragment<IColumn>? HeadTemplate { get; set; }
    [Parameter]
    public virtual RenderFragment<IColumn>? FootTemplate { get; set; }
    [Parameter]
    public string? RowCssClass { get; set; }
    [Parameter]
    public string? ItemCssClass { get; set; }
    [Parameter]
    public string? HeadCssClass { get; set; }
    [Parameter]
    public string? FootCssClass { get; set; }

    public virtual string? PropertyName => null;

    public IColumnEventSignalTarget? ColumnEventReciver => Grid;

    [Parameter]
    public virtual bool Sortable { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        if (Grid != null)
        {
            Grid.AddColumn(this);
        }

        await base.OnInitializedAsync();
    }

    public virtual Task Sort(bool? ascending)
    {
        return Task.CompletedTask;
    }
}
