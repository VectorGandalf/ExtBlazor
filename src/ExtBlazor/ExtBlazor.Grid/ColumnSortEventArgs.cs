namespace ExtBlazor.Grid;
public class ColumnSortEventArgs : IColumnEventArgs
{
    public required string? SortExpression { get; set; }
    public bool? Ascending { get; set; }
}
