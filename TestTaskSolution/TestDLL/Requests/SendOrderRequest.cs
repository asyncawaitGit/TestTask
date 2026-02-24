using TestDLL.Requests.Parameters;

namespace TestDLL.Requests;

public sealed record SendOrderRequest(
    SendOrderParameters CommandParameters
) : IRequest<SendOrderParameters>
{
    public string Command => "SendOrder";
}
