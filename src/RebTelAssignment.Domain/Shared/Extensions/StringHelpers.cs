namespace RebTelAssignment.Domain.Shared.Extensions;

public static class StringHelpers
{
    public static bool HasValue(this string str)
    {
        if (string.IsNullOrWhiteSpace(str) || str.Length == 0)
            return false;
        return true;
    }
}