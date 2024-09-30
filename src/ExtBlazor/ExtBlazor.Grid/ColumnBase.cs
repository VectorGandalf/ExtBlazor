using Microsoft.AspNetCore.Components;

namespace ExtBlazor.Grid;

public abstract class ColumnBase<TItem> : ComponentBase
{
    [CascadingParameter(Name = "ParentGrid")]
    protected Grid<TItem>? Grid { get; set; }
    [Parameter]
    public virtual RenderFragment<TItem>? ChildContent { get; set; }
    [Parameter]
    public string? Title { get; set; }
    [Parameter]
    public bool Sortable { get; set; } = true;
    [Parameter]
    public string? SortString { get; set; }
    [Parameter]
    public bool? DefaultSortColumn { get; set; }
    [Parameter]
    public bool DefaultSortDirectionAsc { get; set; } = true;
    [Parameter]
    public virtual RenderFragment<ColumnBase<TItem>>? HeadTemplate { get; set; }
    [Parameter]
    public virtual RenderFragment<ColumnBase<TItem>>? FootTemplate { get; set; }
    [Parameter]
    public string? RowCssClass { get; set; }
    [Parameter]
    public string? ItemCssClass { get; set; }
    [Parameter]
    public string? HeadCssClass { get; set; }
    [Parameter]
    public string? FootCssClass { get; set; }
    protected override async Task OnInitializedAsync()
    {
        if (Grid != null)
        {
            Grid.AddColumn(this);

            if (HeadTemplate == null)
            {
                HeadTemplate = item => builder =>
                {
                    builder.OpenComponent(0, Grid.DefaultHeadTempate);
                    builder.AddAttribute(1, "Column", item);
                    builder.CloseComponent();
                };
            }
        }

        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) 
        {
            if (DefaultSortColumn == true)
            {
                await Grid!.SignalColumnEvent(new ColumnSortEventArgs
                {
                    Ascending = DefaultSortDirectionAsc,
                    SortExpression = SortString
                });
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public Task Sort(bool? ascending)
    {
        return Grid!.SignalColumnEvent(new ColumnSortEventArgs
        {
            SortExpression = SortString,
            Ascending = ascending
        });
    }
    internal virtual Task OnColumnEventHandler(IColumnEventArgs args) 
        => Task.CompletedTask;
}
