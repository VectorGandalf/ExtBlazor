namespace ExtBlazor.Demo.Client;

public static class Formaters
{
    public static string? FormatPhoneNumber(string? phoneNumber) => phoneNumber?.Insert(3, " (0) ").Insert(11, "-").Insert(14, " ").Insert(17, " ");
    public static string? FormatDate(DateTime date) => date.ToString("yyyy-MM-dd");
    public static string? FormatDateTime(DateTime? dateTime) => dateTime?.ToString("yyyy-MM-dd HH:mm");
}
