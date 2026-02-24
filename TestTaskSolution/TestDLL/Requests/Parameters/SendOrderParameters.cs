using TestDLL.Entities;

namespace TestDLL.Requests.Parameters;

public sealed record SendOrderParameters(
    Guid OrderId,
    IReadOnlyList<OrderMenuItem> MenuItems
);
