# ExtBlazor.Stash
Stash is a statemanager used to stor objects associated with the current uri in a Blazor application.

## Usage example

In Program.cs
```C#
builder.Services.AddStashService();
```

In Page.razor
```Razor
@page("/uri")
@inject IStashService stash

protected override Task OnInitializedAsync()
{
    var stashedObject = stash.Get<SomeType>("someKey");
    if (stashedObject != null)
    {
        @object = stashedObject;
    }

    return base.OnInitializedAsync();
}

private void UseObject()
{
    @object = SomeType();
    stash.Put("someKey", @object);
    //do some thing ... 
}
```