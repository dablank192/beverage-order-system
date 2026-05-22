using System;
using beverage_order_system.Dto;
using FluentValidation;
using MediatR;

namespace beverage_order_system.Feature.Menu.GetMenu;

public record Command (
    int PageIndex,
    int PageSize
) : IRequest<Result>;
public record Result(
    List<MenuDto> Data,
    int TotalRecord,
    int CurrentPage,
    int TotalPage
);

public class DataValidation : AbstractValidator<Command>
{
    public DataValidation()
    {
        RuleFor(t => t.PageIndex)
        .NotNull().WithMessage("Missing Params: PageIndex")
        .GreaterThanOrEqualTo(1).WithMessage("Page Index can not be less than 1");

        RuleFor(t => t.PageSize)
        .NotNull().WithMessage("Missing Params: PageSize")
        .GreaterThanOrEqualTo(1).WithMessage("Page Size can not be less than 1");;
    }
}
