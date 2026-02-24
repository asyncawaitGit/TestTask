using TestDLL.Requests.Parameters;

namespace TestDLL.Requests;

public sealed record GetMenuRequest(
    GetMenuParameters CommandParameters
) : IRequest<GetMenuParameters>
{
    public string Command => "GetMenu";
}
