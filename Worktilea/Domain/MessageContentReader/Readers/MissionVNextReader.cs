using System.Linq;
using Worktile.Models.Message;

namespace Worktile.Domain.MessageContentReader.Readers
{
    class MissionVNextReader : IMessageReader
    {
        public MessageContent Read(Message message)
        {
            return new MessageContent
            {
                Summary = ReadSummary(message),
                Detail = ReadDetail(message)
            };
        }

        public string ReadSummary(Message message) => message.Body.InlineAttachment.Pretext;

        public string ReadDetail(Message message)
        {
            string text = message.Body.InlineAttachment.Pretext;
            text = text + "\r\n" + message.Body.InlineAttachment.Title;
            if (message.Body.InlineAttachment.Fields.Any())
            {
                text += "\r\n";
                foreach (var item in message.Body.InlineAttachment.Fields)
                {
                    text = text + "\r\n" + item.Title + "：" + item.Value;
                }
            }
            return text;
        }
    }
}
