namespace TestDLL.Responses;

public interface IResponse
{
    string Command { get; }
    bool Success { get; }
    string ErrorMessage { get; }
}
