using Sms.Test;

namespace TestDLL.Services.GrpcApiClient;

public sealed class GrpcApiClient : IGrpcApiClient
{
    private readonly SmsTestService.SmsTestServiceClient _client;

    public GrpcApiClient(SmsTestService.SmsTestServiceClient client)
    {
        _client = client;
    }

    public async Task<IReadOnlyList<MenuItem>> GetMenuAsync(bool withPrice, CancellationToken ct = default)
    {
        var request = new Google.Protobuf.WellKnownTypes.BoolValue { Value = withPrice };

        var response = await _client.GetMenuAsync(request, cancellationToken: ct);

        if (!response.Success)
            throw new InvalidOperationException(response.ErrorMessage);

        return response.MenuItems;
    }

    public async Task SendOrderAsync(Guid orderId, IReadOnlyList<OrderItem> items, CancellationToken ct = default)
    {
        var order = new Order
        {
            Id = orderId.ToString()
        };
        order.OrderItems.AddRange(items);

        var response = await _client.SendOrderAsync(order, cancellationToken: ct);

        if (!response.Success)
            throw new InvalidOperationException(response.ErrorMessage);
    }
}
