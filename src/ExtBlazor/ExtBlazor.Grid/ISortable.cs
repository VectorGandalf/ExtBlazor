using ExtBlazor.Core;

namespace ExtBlazor.Grid;
public interface ISortable
{
    IEnumerable<SortExpression> Sort { get; }
    Task Signal(IColumnEventArgs args);
}
