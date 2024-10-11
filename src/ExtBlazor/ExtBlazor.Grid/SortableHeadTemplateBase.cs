namespace ExtBlazor.Grid;

public abstract class SortableHeadTemplateBase : HeadTemplateBase
{
    protected bool Ascending { get; set; }

    protected virtual Task OnClick()
    {
        if (Column.ColumnEventReciver?.Sort.Any(_ => _.Property == Column.PropertyName) ?? false)
        {
            Ascending = !Ascending;
        }
        else
        {
            Ascending = true;
        };

        return Column.Sort(Ascending);
    }

    protected override void OnInitialized()
    {
        Ascending = Column.ColumnEventReciver?
            .Sort
            .FirstOrDefault(i => i.Property == Column.PropertyName)?
            .Ascending ?? false;

        base.OnInitialized();
    }
}