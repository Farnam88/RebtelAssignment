using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RebtelAssignment.Application.Common.PipelineBehaviors;
using RebtelAssignment.Application.Core.EventPublishers;
using RebtelAssignment.Application.Core.Services;
using RebTelAssignment.Domain.Shared.Events;

namespace RebtelAssignment.Application.DependencyInjections;

public static class ApplicationDependencyInjections
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyInfo).Assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyInfo).Assembly);

        services.AddTransient(typeof(IEventPublisher<>), typeof(EventPublisher<>));

        services.AddScoped<IReadingPaceCalculatorService, ReadingPaceCalculatorService>();
        services.AddScoped<ILoanDueDateCalculatorService, LoanDueDateCalculatorService>();
    }
}