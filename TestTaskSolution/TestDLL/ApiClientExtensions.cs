using Microsoft.Extensions.DependencyInjection;
using TestDLL.Services.ApiClient;

namespace TestDLL;

public static class ApiClientExtensions
{
    public static IServiceCollection AddApiClient(
        this IServiceCollection services,
        string baseAddress,
        string username,
        string password)
    {
        // добавляем RequestService с Basic Auth
        services.AddRequestService(baseAddress, username, password);

        // добавляем ApiClient
        services.AddSingleton<IApiClient, ApiClient>();

        return services;
    }
}
