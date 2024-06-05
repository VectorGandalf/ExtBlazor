namespace ExtBlazor.Core;

public interface IPagingationParameters
{
    string? SortExp { get; }
    bool? Asc { get; }
    int? Skip { get; }
    int? Take { get; }
}
