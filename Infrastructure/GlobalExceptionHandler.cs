using System;
using System.Runtime.CompilerServices;
using beverage_order_system.Exception.AuthException;
using beverage_order_system.Exception.MenuException;
using beverage_order_system.Exception.OrderException;
using beverage_order_system.Exception.UserException;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace beverage_order_system.Infrastructure;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment env
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, System.Exception exception, CancellationToken ct)
    {
        logger.LogError(exception, exception.Message);

        var problemDetail = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Error",
            Detail = "An unexpected error has occured",
            Instance = context.Request.Path
        };

        if (exception is DuplicateUsernameException)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            problemDetail.Title = "Username is used";
            problemDetail.Status = StatusCodes.Status200OK;
        }
        else if (exception is InvalidCredentialException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            problemDetail.Title = "Wrong username or password";
            problemDetail.Status = StatusCodes.Status401Unauthorized;
        }
        else if (exception is InvalidRefreshTokenException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            problemDetail.Title = "Invalid Refresh Token";
            problemDetail.Status = StatusCodes.Status401Unauthorized;
        }
        else if (exception is ProductNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            problemDetail.Title = "Menu Item not found";
            problemDetail.Status = StatusCodes.Status200OK;
        }
        else if (exception is UserNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            problemDetail.Title = "User ID not found";
            problemDetail.Status = StatusCodes.Status200OK;
        }
        else if (exception is OrderNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            problemDetail.Title = "OrderId not found";
            problemDetail.Status = StatusCodes.Status404NotFound;
        }

        else if(exception is DeleteOrderJobException)
        {
            problemDetail.Title = "Cron job can not execute";
        }
        
        if (env.IsDevelopment()) //Điều kiện để check xem api có đang ở trong môi trường development không?
        {
            problemDetail.Extensions.Add("Detail", exception.Message);
            problemDetail.Extensions.Add("Traceback", exception.StackTrace);
        }

        await context.Response.WriteAsJsonAsync(problemDetail, ct);

        return true;
    }
}
