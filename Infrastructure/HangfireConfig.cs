using System;
using beverage_order_system.Feature.Order.CleanOrderDaily;
using Hangfire;

namespace beverage_order_system.Infrastructure;

public static class HangfireConfig
{
    public static void RegisterRecurringJob()
    {
        RecurringJob.AddOrUpdate<DeleteOrderJob>(
            "delete-order-job",
            t => t.DeleteOrder(),
            Cron.Daily()
        );

        // Chua Test
    }
}
