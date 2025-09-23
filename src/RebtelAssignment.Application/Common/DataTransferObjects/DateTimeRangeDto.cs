using FluentValidation;

namespace RebtelAssignment.Application.Common.DataTransferObjects;

public class DateTimeRangeDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class TimeRangeValidator : AbstractValidator<DateTimeRangeDto>
{
    public TimeRangeValidator()
    {
        RuleFor(x => x.From)
            .NotNull()
            .NotEqual(default(DateTime));

        RuleFor(x => x.To)
            .NotNull()
            .NotEqual(default(DateTime));

        RuleFor(x => x)
            .Custom(RangeValidator);
    }

    private void RangeValidator(DateTimeRangeDto dto, ValidationContext<DateTimeRangeDto> context)
    {
        if (dto.To <= dto.From)
        {
            context.AddFailure(nameof(dto.To), "Date 'To' should be greater than Date 'From'");
        }
    }
}