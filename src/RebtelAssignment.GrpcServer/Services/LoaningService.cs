using Common;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using LoaningServices;
using MapsterMapper;
using MediatR;
using RebtelAssignment.Application.Core.Loaning.Commands;

namespace RebtelAssignment.GrpcServer.Services;

public class LoaningService : LoaningServices.LoaningService.LoaningServiceBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public LoaningService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task<Empty> LoanBooks(LoanRequestMsg request, ServerCallContext context)
    {
        var resultModel = await _mediator.Send(new LoanBookCommand(request.BatchId.ToList(), request.MemberId),
            context.CancellationToken);
        if (!resultModel.IsSuccess)
            throw resultModel.GetException();
        return new Empty();
    }

    public override Task<LoanMsg> ReturnBooks(ReturnBookRequestMsg request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.Unimplemented,"Not implemented"));
    }
}