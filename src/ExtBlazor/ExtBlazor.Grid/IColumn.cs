namespace ExtBlazor.Grid;

public interface IColumn
{
    string? Title { get; set; }
    string? PropertyName { get; }
    bool Sortable { get; }
    ISortable? ColumnEventReciver { get; }
    Task Sort(bool? ascending);
}