using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RebtelAssignment.Infrastructure.Data.EntityConfigs.Converters;

public static class CustomConverters
{
    internal static readonly ValueConverter<DateOnly, DateTime> DateOnlyConverter =
        new(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v));

    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    internal static readonly ValueConverter<List<string>, string> StringListToJson =
        new(v => JsonSerializer.Serialize(v, JsonOpts),
            v => JsonSerializer.Deserialize<List<string>>(v, JsonOpts) ?? new());
}