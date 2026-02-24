namespace TestDLL.Responses;

public sealed record SendOrderResponse(
    bool Success,
    string ErrorMessage
) : IResponse
{
    public string Command => "SendOrder";
}
