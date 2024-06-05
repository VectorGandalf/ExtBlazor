using Microsoft.AspNetCore.Components;

namespace ExtBlazor.Grid;

public class TemplateColumn<TItem> : ColumnBase<TItem>
{
    [Parameter]
    public RenderFragment<TItem>? ChildContent { get; set; }

    internal override string? GetValue(TItem item)
    {
        throw new NotImplementedException();
    }
}
