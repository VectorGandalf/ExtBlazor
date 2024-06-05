namespace ExtBlazor.Core;

public record PagedSet<TItem>(IEnumerable<TItem> Items, int Total);