namespace TestDLL.Requests;

public interface IRequest<out TParameters>
{
    string Command { get; }
    TParameters CommandParameters { get; }
}
