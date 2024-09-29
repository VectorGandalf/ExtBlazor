using Microsoft.AspNetCore.Components;

namespace ExtBlazor.Grid;
public abstract class HeadTemplateBase<TItem> : ComponentBase
{
    [Parameter]
    public required ColumnBase<TItem> Column { get; set; }

    [CascadingParameter(Name = "ParentGrid")]
    protected Grid<TItem> Grid { get; set; } = null!;
}
