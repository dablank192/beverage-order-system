using System;
using beverage_order_system.Feature.Order.CleanOrderDaily;
using beverage_order_system.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace beverage_order_system.Test.Feature.Order.CleanOrderDaily;

public class CleanOrderSqlTest : IClassFixture<BeverageOrderFactory>
{
    private readonly BeverageOrderFactory _factory;

    public CleanOrderSqlTest(BeverageOrderFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task DeleteOrderJobTest()
    {
        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var createSeqSql = "CREATE SEQUENCE IF NOT EXISTS \"DailyOrderNumber_Seq\" START 1;";
        await db.Database.ExecuteSqlRawAsync(createSeqSql);

        var cronjobService = scope.ServiceProvider.GetRequiredService<DeleteOrderJob>();

        try
        {
            await cronjobService.DeleteOrder();
            Assert.True(true);
        }
        catch(System.Exception ex)
        {
            Assert.Fail($"Clean order job function has an error {ex.Message}");
        }
    }
}
