using Microsoft.AspNetCore.Components;

namespace ExtBlazor.Grid;
public abstract class HeadTemplateBase : ComponentBase
{
    [Parameter]
    public required IColumn Column { get; set; }
}
