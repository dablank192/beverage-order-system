using System.Text;
using beverage_order_system.Feature.Auth;
using beverage_order_system.Infrastructure;
using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using Hangfire;
using Hangfire.PostgreSql;
using beverage_order_system.Feature.Order.CleanOrderDaily;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Add Fluent validation validator

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);


// MediatR config

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Program>();
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
});


// Exception Handler Config

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddHttpContextAccessor();


// Carter config

builder.Services.AddCarter();


// Database config

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseNpgsql(connectionString);
});


// Dependencies register

builder.Services.AddScoped<IHelper, Helper>();


// Authentication config

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey= true,
        ValidateIssuer= false,
        ValidateAudience= false,
        ValidateLifetime= true,
        IssuerSigningKey= new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
    
    option.Events = new JwtBearerEvents
    {
        OnMessageReceived= context =>
        {
            var accessToken = context.Request.Cookies["x-access-token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


// Hangfire config

builder.Services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(option =>
    {
        option.UseNpgsqlConnection(connectionString);
    });
});

builder.Services.AddHangfireServer();

builder.Services.AddTransient<DeleteOrderJob>();



// BUILD

var app = builder.Build();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

using (var scope = app.Services.CreateScope())
{
    var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    HangfireConfig.RegisterRecurringJob(jobManager);
}

app.UseHangfireDashboard();

// HTTP request pipeline Config
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.Run();
