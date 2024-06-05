namespace ExtBlazor.Core;

public class PaginationParameters : IPagingationParameters
{
    public string? SortExp { get; set; }
    public bool? Asc { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}
