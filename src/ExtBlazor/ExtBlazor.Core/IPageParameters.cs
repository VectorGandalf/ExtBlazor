namespace ExtBlazor.Core;

public interface IPageParameters
{
    IEnumerable<SortExpression>? Sort { get; set; }
    int? Skip { get; set; }
    int? Take { get; set; }
}

public record SortExpression(string Property, bool? Ascending = true);