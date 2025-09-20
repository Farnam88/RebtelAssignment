using MediatR;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.Application.Common.Abstractions.Commands;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}

public abstract class BaseCommandHandler<TCommand, TResponse>
    : ICommandHandler<TCommand, ResultModel<TResponse>>
    where TCommand : BaseCommand<TResponse>
{
    protected readonly IUnitOfWork Uow;

    protected BaseCommandHandler(IUnitOfWork unitOfWork)
    {
        Uow = unitOfWork;
    }

    public abstract Task<ResultModel<TResponse>> Handle(TCommand request, CancellationToken ct = default);
}