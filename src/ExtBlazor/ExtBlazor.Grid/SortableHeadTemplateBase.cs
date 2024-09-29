namespace ExtBlazor.Grid;

public abstract class SortableHeadTemplateBase<TItem> : HeadTemplateBase<TItem>
{
    protected bool Ascending { get; set; }
    protected virtual Task OnClick()
    {
        if (Grid.SortExpression == Column.SortColumn)
        {
            Ascending = !Ascending;
        }
        else 
        {
            Ascending = true;
        };
        return Column.Sort(Ascending);
    }
}