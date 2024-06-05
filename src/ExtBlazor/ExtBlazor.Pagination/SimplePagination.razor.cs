using Microsoft.AspNetCore.Components;

namespace ExtBlazor.Paginators;
public partial class SimplePagination
{
    [Parameter, EditorRequired]
    public required Core.Pagination Pagination { get; set; }
    
    [Parameter]
    public string? NavCssClass { get; set; }

    [Parameter]
    public string? ButtonCssClass { get; set; }

    [Parameter]
    public string? ActiveCssClass { get; set; }

    [Parameter]
    public int Padding { get; set; } = 13;
}
