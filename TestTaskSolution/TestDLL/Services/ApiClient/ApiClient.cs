using TestDLL.DTO;
using TestDLL.Entities;
using TestDLL.Requests;
using TestDLL.Requests.Parameters;
using TestDLL.Responses;
using TestDLL.Services.Request;

namespace TestDLL.Services.ApiClient;

public sealed class ApiClient : IApiClient
{
    private readonly IRequestService _requestService;

    public ApiClient(IRequestService requestService)
    {
        _requestService = requestService;
    }

    public async Task<IReadOnlyList<MenuItemDto>> GetMenuAsync(bool withPrice, CancellationToken ct = default)
    {
        var request = new GetMenuRequest(
            new GetMenuParameters(WithPrice: withPrice)
        );

        var response = await _requestService.SendAsync<GetMenuResponse>(request, ct);

        if (!response.Success)
            throw new InvalidOperationException(response.ErrorMessage);

        return response.Data?.MenuItems ?? Array.Empty<MenuItemDto>();
    }

    public async Task SendOrderAsync(Guid orderId, IReadOnlyList<OrderMenuItem> items, CancellationToken ct = default)
    {
        var request = new SendOrderRequest(
            new SendOrderParameters(orderId, items)
        );

        var response = await _requestService.SendAsync<SendOrderResponse>(request, ct);

        if (!response.Success)
            throw new InvalidOperationException(response.ErrorMessage);
    }
}
