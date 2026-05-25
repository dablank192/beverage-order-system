using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace beverage_order_system.Test;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> option,
        ILoggerFactory logger,
        UrlEncoder encoder
    ) : base(option, logger, encoder)
    {}

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim("UserId", Guid.NewGuid().ToString()),
            new Claim("Username", "Testing"),
            new Claim(ClaimTypes.Name, "Testing")
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestAuth");

        return Task.FromResult(AuthenticateResult.Success(ticket));

        //nhớ hỏi AI giải thích đoạn code này
    }
}
