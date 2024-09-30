namespace ExtBlazor.Grid;
public class ColumnSortEventArgs : IColumnEventArgs
{
    public required string? SortString { get; set; }
    public bool? Ascending { get; set; }
}
