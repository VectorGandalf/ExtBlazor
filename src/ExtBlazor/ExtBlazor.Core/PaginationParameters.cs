namespace ExtBlazor.Core;

public class PaginationParameters : IPageParameters
{
    public string? Sort { get; set; }
    public bool? Asc { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}
