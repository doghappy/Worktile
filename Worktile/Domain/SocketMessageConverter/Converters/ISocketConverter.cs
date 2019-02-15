namespace Worktile.Domain.SocketMessageConverter.Converters
{
    public interface ISocketConverter
    {
        bool IsMatch(string rawMessage);
        string Read(string rawMessage);
        string Process(string message);
    }
}
