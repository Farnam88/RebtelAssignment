namespace RebTelAssignment.Domain.Shared.Extensions;

public static class DateTimeExtensions
{
    public static DateOnly ToDateOnly(this DateTime date)
    {
        return new DateOnly(date.Year, date.Month, date.Day);
    }

    public static decimal ToDecimal(this TimeSpan timeSpan)
    {
        var hours = timeSpan.Hours;
        var minutes = timeSpan.Minutes;
        var seconds = timeSpan.Seconds;
        return hours + (decimal)minutes / 60 + (decimal)seconds / 3600;
    }
}