using ExtBlazor.Core;

namespace ExtBlazor.Grid;
public class ColumnSortEventArgs : IColumnEventArgs
{
    public required IEnumerable<SortExpression> SortExpressions { get; set; }
}
