using System;
using FluentValidation;
using MediatR;

namespace beverage_order_system.Feature.Menu.UpdateMenuItem;

public record SubCommand(
    string? Name,
    int? CategoryId,
    decimal? BasePrice,
    bool? IsAvailable
);
public record Command(
    int ProductId,
    SubCommand Data
) : IRequest<Result>;
public record Result();

public class DataValidation : AbstractValidator<SubCommand>
{
    public DataValidation()
    {
        When(t => t.Name != null, () =>
        {
            RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Product name must not be empty");
        });
    }
}

public class CommandValidation : AbstractValidator<Command>
{
    public CommandValidation()
    {
        RuleFor(t => t.ProductId)
        .GreaterThan(0)
        .WithMessage("Validator must be greater than 0");
        
        RuleFor(t => t.Data).SetValidator(new DataValidation());
    }
}
 