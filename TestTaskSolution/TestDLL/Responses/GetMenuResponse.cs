using TestDLL.Responses.Dates;

namespace TestDLL.Responses;

public sealed record GetMenuResponse(
    bool Success,
    string ErrorMessage,
    GetMenuData? Data
) : IResponse<GetMenuData>
{
    public string Command => "GetMenu";
}
