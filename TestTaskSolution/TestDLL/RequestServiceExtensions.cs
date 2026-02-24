using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text;
using TestDLL.Services.Request;

namespace TestDLL;

public static class RequestServiceExtensions
{
    public static IServiceCollection AddRequestService(
            this IServiceCollection services,
            string baseAddress,
            string username,
            string password)
    {
        services.AddHttpClient<IRequestService, RequestService>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var credentials = $"{username}:{password}";
            var base64 = Convert.ToBase64String(
                Encoding.ASCII.GetBytes(credentials));

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", base64);
        });

        return services;
    }
}
