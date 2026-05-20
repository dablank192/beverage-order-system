using System;
using beverage_order_system.Dto;

namespace beverage_order_system.Model;

public class Order
{
    public Guid Id {get; set;}
    public int DailyOrderNumber {get; set;} //Order number để nhận diện đơn của KH, reset về 1 mỗi đầu ngày
    public decimal TotalAmount {get; set;}
    public OrderStatus Status {get; set;}
    public PaymentStatus PayStatus {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public List<OrderItems>? OrderItems {get; set;}
}
