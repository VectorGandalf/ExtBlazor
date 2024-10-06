namespace ExtBlazor.Grid;

public interface IColumn
{
    string? Title { get; set; }
    string? PropertyName { get; }
    bool Sortable { get; }
    IColumnEventSignalTarget? ColumnEventReciver { get; }
    Task Sort(bool? ascending);
}