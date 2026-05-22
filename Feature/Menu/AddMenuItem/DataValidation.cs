using System;
using FluentValidation;
using MediatR;

namespace beverage_order_system.Feature.Menu.AddMenuItem;

public record Command(
    string? Name,
    int CategoryId,
    decimal? Price,
    string? ProductImageUrl,
    bool? IsAvailable
) : IRequest<Result>;
public record Result();

public class DataValidation : AbstractValidator<Command>
{
    public DataValidation()
    {
        RuleFor(t => t.Name)
        .MaximumLength(50);
    }
}
