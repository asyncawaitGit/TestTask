using TestDLL.DTO;
using TestDLL.Entities;

namespace TestDLL.Services.ApiClient;

public interface IApiClient
{
    Task<IReadOnlyList<MenuItemDto>> GetMenuAsync(bool withPrice, CancellationToken ct = default);
    Task SendOrderAsync(Guid orderId, IReadOnlyList<OrderMenuItem> items, CancellationToken ct = default);
}
