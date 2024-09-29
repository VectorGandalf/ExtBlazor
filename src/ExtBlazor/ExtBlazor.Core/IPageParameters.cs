namespace ExtBlazor.Core;

public interface IPageParameters
{
    string? Sort { get; set; }
    int? Skip { get; set; }
    int? Take { get; set; }
}
