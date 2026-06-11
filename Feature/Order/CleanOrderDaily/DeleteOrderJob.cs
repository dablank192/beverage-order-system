using System;
using beverage_order_system.Exception.OrderException;
using beverage_order_system.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace beverage_order_system.Feature.Order.CleanOrderDaily;

public class DeleteOrderJob(
    AppDbContext dbContext
)

{
    public async Task DeleteOrder()
    {
        var sqlCommand = "ALTER SEQUENCE \"DailyOrderNumber_Seq\" RESTART WITH 1;";

        try
        {
            await dbContext.Database.ExecuteSqlRawAsync(sqlCommand);
        }
        catch (System.Exception ex)
        {
            throw new System.Exception($"Error encounter when running cronjob, {ex}");
        }
    }

}
