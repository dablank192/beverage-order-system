using System;
using beverage_order_system.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace beverage_order_system.Test;

public class BeverageOrderFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer dbContainer = new PostgreSqlBuilder()
    .WithImage("postgres:15-alpine")
    .WithDatabase("test-db")
    .WithUsername("postgres")
    .WithPassword("postgres")
    .Build();

    public async Task InitializeAsync() => await dbContainer.StartAsync();

    public new async Task DisposeAsync() => await dbContainer.DisposeAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder) 
    //can thiệp vào quá trình khởi động ứng dụng,
    //và thay đổi các cấu hình thiết lập môi trường chạy của ứng dụng
    //trước khi ứng dụng được chạy thực tếthông qua IWebHostBuilder
    {
        builder.ConfigureServices(service =>
        {
            service.RemoveAll(typeof(DbContextOptions<AppDbContext>)); //xóa connection string ở DbContext gốc đi

            service.AddDbContext<AppDbContext>(option =>
            {
                option.UseNpgsql(dbContainer.GetConnectionString());
            }); //gán connection string của container database vào

            var sp = service.BuildServiceProvider(); //chuyển danh sách Service đã đc đăng ký thành một object có thể gọi được
            using var scope = sp.CreateScope(); //tạo một scope tạm thời mục đích là để bước sau lấy ra DbContext
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>(); //Lấy ra AppDbContext từ scope
            db.Database.EnsureCreated();

            service.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = "TestAuth";
                op.DefaultChallengeScheme = "TestAuth";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestAuth", option => { });
        });
    }
}
