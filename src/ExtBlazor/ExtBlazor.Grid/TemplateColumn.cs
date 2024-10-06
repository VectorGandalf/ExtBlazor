namespace ExtBlazor.Grid;

public class TemplateColumn<TItem> : ColumnBase<TItem>
{
    protected override async Task OnInitializedAsync()
    {
        if (HeadTemplate == null)
        {
            HeadTemplate = item => builder =>
            {
                builder.AddContent(0, Title);
            };
        }

        await base.OnInitializedAsync();
    }
}
