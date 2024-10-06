using ExtBlazor.Core;

namespace ExtBlazor.Grid;
public interface IColumnEventSignalTarget
{
    IEnumerable<SortExpression> Sort { get; }
    Task Signal(IColumnEventArgs args);
}
