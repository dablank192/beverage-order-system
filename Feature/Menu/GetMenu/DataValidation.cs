using System;
using beverage_order_system.Dto;
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

public class DataValidation
{

}
