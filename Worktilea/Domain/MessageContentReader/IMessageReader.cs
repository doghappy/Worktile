using Worktile.Models.Message;

namespace Worktile.Domain.MessageContentReader
{
    interface IMessageReader
    {
        MessageContent Read(Message message);
        string ReadSummary(Message message);
        string ReadDetail(Message message);
    }
}
