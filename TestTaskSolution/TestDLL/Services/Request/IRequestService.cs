using TestDLL.Requests;
using TestDLL.Responses;

namespace TestDLL.Services.Request;

public interface  IRequestService
{
    Task<TResponse> SendAsync<TResponse>(
        IRequest<object> request,
        CancellationToken cancellationToken = default
    ) where TResponse : IResponse;
}
