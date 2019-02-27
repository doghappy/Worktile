using Worktile.Common;
using Worktile.Models.Message;

namespace Worktile.Domain.MessageContentReader.Readers
{
    class DefaultReader : IMessageReader
    {
        public MessageContent Read(Message message)
        {
            return new MessageContent
            {
                Summary = ReadSummary(message),
                Detail = ReadDetail(message)
            };
        }

        public string ReadSummary(Message message) => WtMarkdown.FormatForMessage(message.Body.Content);
        public string ReadDetail(Message message) => WtMarkdown.FormatForMessage(message.Body.Content);
    }
}
