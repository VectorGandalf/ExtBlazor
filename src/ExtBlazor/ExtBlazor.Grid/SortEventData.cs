namespace ExtBlazor.Grid;
public class SortEventData : IColumnEventArgs
{
    public required string? SortExpression { get; set; }
    public bool? Ascending { get; set; }
}
