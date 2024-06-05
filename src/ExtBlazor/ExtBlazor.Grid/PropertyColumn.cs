using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace ExtBlazor.Grid;

public class PropertyColumn<TItem, TProperty> : ColumnBase<TItem>
{
    [Parameter, EditorRequired]
    public required Expression<Func<TItem, TProperty>> Property { get; set; }

    [Parameter]
    public Func<TProperty, string?>? Format { get; set; }

    private Func<TItem, TProperty>? compiledProperty;
    private string? propertyName;
    public override RenderFragment<ColumnBase<TItem>>? HeadTemplate { get; set; } = item
        => builder =>
        {
            builder.OpenComponent(0, typeof(PropertyColumnDefaultHeadTemplate<TItem, TProperty>));
            builder.AddAttribute(1, "Column", (PropertyColumn<TItem, TProperty>)item);
            builder.CloseComponent();
        };

    protected override void OnParametersSet()
    {
        compiledProperty = Property.Compile();
        if (Property != null && Property.Body is MemberExpression memberExpression) 
        {
            propertyName = memberExpression.Member.Name;
        }

        if (Title == null && propertyName != null)
        {
            Title = propertyName;
        }

        if (SortColumn == null) 
        {
            SortColumn = propertyName;
        }

        base.OnParametersSet();
    }

    internal override string? GetValue(TItem item)
    {
        if (compiledProperty != null)
        {
            var val = compiledProperty(item);
            return Format != null 
                ? Format(val)
                : val?.ToString();
        }

        return null;
    }

    public Task Sort(bool? ascending) 
    {
        return Grid!.SignalColumnEvent(new SortEventData
        {
            SortExpression = propertyName,
            Ascending = ascending
        });
    }
}
