using System.Text;
using System.Text.Json;
using TestDLL.Requests;
using TestDLL.Responses;

namespace TestDLL.Services.Request;

public class RequestService : IRequestService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public RequestService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<TResponse> SendAsync<TResponse>(
        IRequest<object> request,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse
    {
        var json = JsonSerializer.Serialize(request, _jsonOptions);

        using var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        using var response = await _httpClient.PostAsync(
            "", // endpoint, базовый адрес задаётся в DI
            content,
            cancellationToken
        );

        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

        var result = JsonSerializer.Deserialize<TResponse>(
            responseJson,
            _jsonOptions
        );

        if (result is null)
            throw new InvalidOperationException("Response deserialization failed");

        return result;
    }
}
