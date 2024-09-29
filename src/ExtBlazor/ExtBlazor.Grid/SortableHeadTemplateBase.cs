namespace ExtBlazor.Grid;

public abstract class SortableHeadTemplateBase<TItem> : HeadTemplateBase<TItem>
{
    protected bool Ascending { get; set; }
    protected virtual Task OnClick()
    {
        if (Grid.SortExpression == Column.SortString)
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
        Ascending = Column.DefaultSortDirectionAsc;
        base.OnInitialized();
    }
}