using RebTelAssignment.Domain.Shared.Extensions;

namespace RebtelAssignment.Application.Core.Services;

public interface IReadingPaceCalculatorService
{
    MemberOverallReadingPaceModel CalculatePaceForBook(IList<LoanItemAggregatedModel> input);
}

public class ReadingPaceCalculatorService : IReadingPaceCalculatorService
{
    public MemberOverallReadingPaceModel CalculatePaceForBook(IList<LoanItemAggregatedModel> input)
    {
        foreach (var item in input)
        {
            if (item.ReturnedAt == null)
                continue;
            var durationByHour = TimeSpan.FromTicks(item.LoanedAt.Ticks - item.ReturnedAt.Value.Ticks).ToDecimal();
            if (durationByHour == 0)
            {
                item.SetReadingPace(item.Pages);
                continue;
            }

            var readingPacePerBook = durationByHour / item.Pages;
            item.SetReadingPace(readingPacePerBook);
        }

        return new MemberOverallReadingPaceModel
        {
            OverallReadingPace = input.Sum(s => s.ReadingPace) / input.Count,
            LoanItems = input
        };
    }
}

public class MemberOverallReadingPaceModel
{
    public decimal OverallReadingPace { get; set; }
    public required IList<LoanItemAggregatedModel> LoanItems { get; set; }
}

public class LoanItemAggregatedModel
{
    private decimal _readingPace;
    public long BatchId { get; set; }
    public required string Title { get; set; }
    public long BookId { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public DateTime LoanedAt { get; set; }
    public long Pages { get; set; }

    public decimal ReadingPace
    {
        get => _readingPace;
        set => _readingPace = value;
    }

    public void SetReadingPace(decimal readingPace)
    {
        _readingPace = readingPace;
    }
}