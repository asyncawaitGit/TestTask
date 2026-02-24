namespace TestDLL.Responses;

public interface IResponse<out TData> : IResponse
{
    TData? Data { get; }
}
