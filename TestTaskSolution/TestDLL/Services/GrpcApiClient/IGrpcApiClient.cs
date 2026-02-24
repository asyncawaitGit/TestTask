namespace TestDLL.Services.GrpcApiClient;

public interface IGrpcApiClient
{
    Task<IReadOnlyList<Sms.Test.MenuItem>> GetMenuAsync(bool withPrice, CancellationToken ct = default);
    Task SendOrderAsync(Guid orderId, IReadOnlyList<Sms.Test.OrderItem> items, CancellationToken ct = default);
}
