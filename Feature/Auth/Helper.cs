using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using beverage_order_system.Model;
using Microsoft.IdentityModel.Tokens;

namespace beverage_order_system.Feature.Auth;

public interface IHelper
{
    public string GenerateJwtToken(User user);
}


public class Helper(
    IConfiguration config
) : IHelper
{
    public string GenerateJwtToken(User user)
    {
        var jwt = config.GetSection("Jwt");

        var claim = new List<Claim>
        {
            new("UserId", user.Id.ToString()),
            new("Username", user.Username.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            signingCredentials: credentials,
            claims: claim,
            expires: DateTime.UtcNow.AddMinutes(15)
        );

        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }
}
