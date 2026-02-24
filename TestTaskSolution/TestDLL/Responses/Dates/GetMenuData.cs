using TestDLL.DTO;

namespace TestDLL.Responses.Dates;

public sealed record GetMenuData(
    IReadOnlyList<MenuItemDto> MenuItems
);
