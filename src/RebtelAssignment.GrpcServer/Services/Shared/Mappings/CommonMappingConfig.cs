using Common;
using Google.Protobuf.WellKnownTypes;
using Mapster;
using RebtelAssignment.Application.Common.DataTransferObjects;

namespace RebtelAssignment.GrpcServer.Services.Shared.Mappings;

public class CommonMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<decimal, DecimalValue>()
            .Map(des => des,
                src => new DecimalValue(src));

        config.NewConfig<decimal?, DecimalValue?>()
            .Map(des => des,
                src => src.HasValue ? new DecimalValue(src) : null);

        config.NewConfig<DateTime, Timestamp>()
            .Map(dest => dest,
                src => Timestamp.FromDateTime(DateTime.SpecifyKind(src, DateTimeKind.Utc)));

        config.NewConfig<DateTime?, Timestamp>()
            .Map(dest => dest,
                src => src.HasValue
                    ? Timestamp.FromDateTime(DateTime.SpecifyKind(src.Value, DateTimeKind.Utc))
                    : null);

        config.NewConfig<DateOnly, Timestamp>()
            .Map(dest => dest,
                src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.ToDateTime(TimeOnly.MinValue),
                    DateTimeKind.Utc)));

        config.NewConfig<DateOnly?, Timestamp>()
            .Map(dest => dest,
                src => src.HasValue
                    ? Timestamp.FromDateTime(DateTime.SpecifyKind(src.Value.ToDateTime(TimeOnly.MinValue),
                        DateTimeKind.Utc))
                    : null);

        config.NewConfig<Timestamp, DateTime>()
            .Map(dest => dest,
                src => src.ToDateTime());

        config.NewConfig<Timestamp, DateTime?>()
            .Map(dest => dest,
                src => src.ToDateTime());
        
        config.NewConfig<TimeRangeMsg, DateTimeRangeDto>()
            .Map(dest => dest.From,
                src => src.From.Adapt<DateTime>())
            .Map(dest => dest.To,
                src => src.To.Adapt<DateTime>());


        TypeAdapterConfig.GlobalSettings.Default.RequireDestinationMemberSource(true);
        TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
    }
}