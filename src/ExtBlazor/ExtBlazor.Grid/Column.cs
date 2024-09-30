using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace ExtBlazor.Grid;

public class Column<TItem, TProperty> : TemplateColumn<TItem>
{
    [Parameter, EditorRequired]
    public required Expression<Func<TItem, TProperty>> Property { get; set; }
    [Parameter]
    public Func<TProperty, string?>? Format { get; set; }
    [Parameter]
    public Func<TItem?, string?>? Href { get; set; }

    private Func<TItem, TProperty>? compiledProperty;
    private string? propertyName;

    protected override void OnInitialized()
    {
        if (ChildContent == null)
        {
            ChildContent = item => builder =>
            {
                builder.OpenComponent(0, typeof(PropertyColumnTemple<TItem, TProperty>));
                builder.AddAttribute(1, "Column", this);
                builder.AddAttribute(2, "Item", item);
                builder.CloseComponent();
            };
        }
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        compiledProperty = Property.Compile();
        if (Property != null && Property.Body is MemberExpression memberExpression)
        {
            propertyName = string.Join('.', memberExpression.ToString().Split('.').Skip(1));
        }

        if (Title == null && propertyName != null)
        {
            Title = propertyName;
        }

        if (SortString == null)
        {
            SortString = propertyName;
        }

        base.OnParametersSet();
    }

    internal string? GetValue(TItem? item)
    {
        if (compiledProperty != null && item != null)
        {
            var val = compiledProperty(item);
            return Format != null
                ? Format(val)
                : val?.ToString();
        }

        return null;
    }
}
