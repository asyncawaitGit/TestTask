using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Sms.Test;
using TestDLL.Services.GrpcApiClient;

namespace TestDLL;

public static class GrpcApiClientExtensions
{
    public static IServiceCollection AddGrpcApiClient(
        this IServiceCollection services,
        string grpcUrl)
    {
        // создаём канал
        var channel = GrpcChannel.ForAddress(grpcUrl);

        // регистрируем gRPC клиент
        var client = new SmsTestService.SmsTestServiceClient(channel);

        services.AddSingleton(client);
        services.AddSingleton<IGrpcApiClient, GrpcApiClient>();

        return services;
    }
}
