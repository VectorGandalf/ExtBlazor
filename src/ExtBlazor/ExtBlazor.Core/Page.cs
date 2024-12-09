namespace ExtBlazor.Core;

public record Page<TItem>(IEnumerable<TItem> Items, int Total);