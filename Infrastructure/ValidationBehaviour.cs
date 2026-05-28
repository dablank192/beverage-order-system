using System;
using beverage_order_system.Feature.Auth.UserLogin;
using FluentValidation;
using MediatR;

namespace beverage_order_system.Infrastructure;

public class ValidationBehaviour<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull

{
    public async Task<TResponse> Handle (TRequest req, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!validators.Any()) return await next(ct);

        var context = new ValidationContext<TRequest>(req);

        var validationTask = new List<Task<FluentValidation.Results.ValidationResult>>();

        foreach(var validator in validators)
        {
            var task = validator.ValidateAsync(context, ct);
            validationTask.Add(task);   
        }

        var validationResult = await Task.WhenAll(validationTask);

        var errors = validationResult.Where(t => t.Errors.Any())
        .SelectMany(t => t.Errors)
        .ToList();

        if (errors.Any()) throw new FluentValidation.ValidationException(errors);

        return await next(ct);
    }
}
