using Sms.Test;

namespace TestDLL.Entities;

public class Order
{
    public Guid Id { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}
