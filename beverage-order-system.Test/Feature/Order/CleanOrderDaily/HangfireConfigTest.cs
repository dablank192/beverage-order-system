using System;
using beverage_order_system.Infrastructure;
using Hangfire;
using Hangfire.Common;
using Moq;
using Org.BouncyCastle.Asn1.Cms;

namespace beverage_order_system.Test.Feature.Order.CleanOrderDaily;

public class HangfireConfigTest
{
    [Fact]
    public void RegisterRecurringJobTest()
    {
        var mockJobManager = new Mock<IRecurringJobManager>();

        HangfireConfig.RegisterRecurringJob(mockJobManager.Object);

        mockJobManager.Verify(t => t.AddOrUpdate(
            "delete-order-job",
            It.IsAny<Job>(),
            Cron.Daily(),
            It.IsAny<RecurringJobOptions>()
        ), Times.Once);
    }
}
