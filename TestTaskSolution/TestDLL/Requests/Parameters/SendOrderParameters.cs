using TestDLL.Entities;

namespace TestDLL.Requests.Parameters;

public sealed record SendOrderParameters
{
    public Guid OrderId { get; set; }
    List<OrderMenuItem>? MenuItems { get; set; }
}
