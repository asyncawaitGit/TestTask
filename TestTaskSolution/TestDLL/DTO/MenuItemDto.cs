namespace TestDLL.DTO;

public sealed record MenuItemDto(
    string Id,
    string Article,
    string Name,
    decimal Price,
    bool IsWeighted,
    string FullPath,
    IReadOnlyList<string> Barcodes
);
