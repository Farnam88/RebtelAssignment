namespace RebTelAssignment.Domain.Shared.Extensions;

public static class DateExtensions
{
    public static DateOnly ToDateOnly(this DateTime date)
    {
        return new DateOnly(date.Year, date.Month, date.Day);
    }
}