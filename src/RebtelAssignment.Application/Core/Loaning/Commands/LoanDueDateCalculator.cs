using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Models.Enums;
using RebTelAssignment.Domain.Shared.Extensions;

namespace RebtelAssignment.Application.Core.Loaning.Commands;

public interface ILoanDueDateCalculatorService
{
    DateOnly CalculateLoanDueDate(LoanSetting loanSetting, DateTime loanDate);
}

public class LoanDueDateCalculatorService : ILoanDueDateCalculatorService
{
    public DateOnly CalculateLoanDueDate(LoanSetting loanSetting, DateTime loanDate)
    {
        DateTime dueDateTime;
        switch (loanSetting.LoanDurationUnitType)
        {
            case LoanDurationUnitType.Day:
                dueDateTime = loanDate.AddDays(loanSetting.Value);
                break;
            case LoanDurationUnitType.Month:
                dueDateTime = loanDate.AddMonths(loanSetting.Value);
                break;
            case LoanDurationUnitType.Week:
                dueDateTime = loanDate.AddDays(loanSetting.Value * 7);
                break;
            case LoanDurationUnitType.Year:
                dueDateTime = loanDate.AddYears(loanSetting.Value);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return dueDateTime.ToDateOnly();
    }
}