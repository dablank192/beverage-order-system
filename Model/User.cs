using System;

namespace beverage_order_system.Model;

public class User
{
    public Guid Id {get; set;}
    public required string Username {get; set;}
    public required string Password {get; set;}
    public string? Email {get; set;}
    public string? PhoneNumber {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public List<RefreshToken>? RefreshToken {get; set;} 
}
