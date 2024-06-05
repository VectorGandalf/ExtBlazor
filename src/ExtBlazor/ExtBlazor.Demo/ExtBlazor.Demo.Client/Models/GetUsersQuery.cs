using ExtBlazor.Core;
using System.Diagnostics.CodeAnalysis;

namespace ExtBlazor.Demo.Client.Models;

public class GetUsersQuery : IPagingationParameters, IQuery
{
    public string? Search { get; set; }
    public string? SortExp { get; set; }
    public bool? Asc { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}
