using ExtBlazor.Core;
using Microsoft.AspNetCore.Components;

namespace ExtBlazor.Paginators;
public partial class SimplePagination
{
    [Parameter, EditorRequired]
    public required Pagination Pagination { get; set; }

    [Parameter]
    public string? NavCssClass { get; set; }

    [Parameter]
    public string? ButtonCssClass { get; set; }

    [Parameter]
    public string? ActiveCssClass { get; set; }

    [Parameter]
    public string? SelectCssClass { get; set; }

    [Parameter]
    public int Padding { get; set; } = 13;

    [Parameter]
    public IEnumerable<int> PageSizes { get; set; } = [1, 2, 5, 10, 25, 50, 75, 100];
}
