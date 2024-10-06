using Microsoft.AspNetCore.Components;
using System.ComponentModel.Design;
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
    [Parameter]
    public override bool Sortable { get; set; } = true;

    public override string? PropertyName => propertyName;

    private Func<TItem, TProperty>? compiledProperty;
    private string? propertyName;

    protected override void OnInitialized()
    {
        if (Grid != null)
        {
            if (HeadTemplate == null)
            {
                HeadTemplate = item => builder =>
                {
                    builder.OpenComponent(0, Grid.DefaultHeadTempate);
                    builder.AddAttribute(3, "Column", item);
                    builder.CloseComponent();
                };
            }
        }

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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public override Task Sort(bool? ascending)
    {
        if (propertyName == null || Grid == null)
        {
            return Task.CompletedTask;
        }

        if (ascending == null)
        {
            return Grid.Signal(new ColumnSortEventArgs
            {
                SortExpressions = Grid.Sort.Where(s => s.Property != PropertyName)
            });
        }

        if (Grid.MultiColumnSort)
        {
            return Grid.Signal(new ColumnSortEventArgs
            {
                SortExpressions = Grid.Sort
                    .Where(s => s.Property != PropertyName)
                    .Prepend(new Core.SortExpression(propertyName, ascending))
            });
        }
        else
        {
            return Grid.Signal(new ColumnSortEventArgs
            {
                SortExpressions = [new(propertyName, ascending)]
            });
        }
    }
}
