using FluentValidation;
using MediatR;
using RebTelAssignment.Domain.Shared.CustomExceptions;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.Application.Common.PipelineBehaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest,TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var errorsDetails = _validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) =>
                    new ErrorDetail(propertyName ?? "", 
                        string.Join(", ", errorMessages.Distinct().ToArray()), true))
            .ToList();

        if (errorsDetails.Count != 0)
        {
            throw new DataValidationException(errorDetails: errorsDetails);
        }

        return await next();
    }
}