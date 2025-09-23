using Grpc.Core;
using InsightServices;
using MapsterMapper;
using MediatR;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebtelAssignment.Application.Core.Insights.Queries;

namespace RebtelAssignment.GrpcServer.Services;

public class InsightsService : InsightServices.InsightsService.InsightsServiceBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public InsightsService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task<TopLoanedBooksResponseMsg> TopLoanedBooks(TopLoanedBooksRequestMsg request,
        ServerCallContext context)
    {
        var resultModel = await _mediator.Send(new TopLoanedBookQuery
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
            },
            context.CancellationToken);
        if (!resultModel.IsSuccess)
            throw resultModel.GetException();

        return _mapper.Map<TopLoanedBooksResponseMsg>(resultModel.Result);
    }

    public override async Task<TopMembersByLoanCountResponseMsg> TopMembersByLoanCount(
        TopMembersByLoanCountRequestMsg request, ServerCallContext context)
    {
        var dateTimeRange = _mapper.Map<DateTimeRangeDto>(request.Range);
        var resultModel = await _mediator.Send(new TopMembersByLoanQuery
            (new DateTimeRangeDto
            {
                From = DateTime.SpecifyKind(request.Range.From.ToDateTime(), DateTimeKind.Utc),
                To = DateTime.SpecifyKind(request.Range.To.ToDateTime(), DateTimeKind.Utc),
            })
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
            },
            context.CancellationToken);
        if (!resultModel.IsSuccess)
            throw resultModel.GetException();

        return _mapper.Map<TopMembersByLoanCountResponseMsg>(resultModel.Result);
    }

    public override async Task<MemberReadingPaceResponseMsg> MemberReadingPace(MemberReadingPaceRequestMsg request,
        ServerCallContext context)
    {
        var resultModel = await _mediator.Send(new MemberReadingPaceQuery(request.MemberId),
            context.CancellationToken);
        if (!resultModel.IsSuccess)
            throw resultModel.GetException();

        return _mapper.Map<MemberReadingPaceResponseMsg>(resultModel.Result);
    }

    public override async Task<LoaningPatternByBookResponseMsg> LoaningPatternByBooks(
        LoaningPatternByBookRequestMsg request,
        ServerCallContext context)
    {
        var resultModel = await _mediator.Send(new LoaningPatternByBookQuery
                (request.BookId)
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                },
            context.CancellationToken);
        if (!resultModel.IsSuccess)
            throw resultModel.GetException();

        return _mapper.Map<LoaningPatternByBookResponseMsg>(resultModel.Result);
    }
}