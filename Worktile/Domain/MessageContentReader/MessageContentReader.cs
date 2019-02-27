using Worktile.Domain.MessageContentReader.Readers;
using Worktile.Enums.Message;
using Worktile.Models.Message;

namespace Worktile.Domain.MessageContentReader
{
    static class MessageContentReader
    {
        private static IMessageReader GetReader(Message message)
        {
            IMessageReader reader = null;
            switch (message.Type)
            {
                case MessageType.Attachment:
                    reader = new AttachmentReader();
                    break;
                case MessageType.MissionVNext:
                    reader = new MissionVNextReader();
                    break;
                default:
                    reader = new DefaultReader();
                    break;
            }
            return reader;
        }

        public static MessageContent Read(Message message)
        {
            IMessageReader reader = GetReader(message);
            return reader.Read(message);
        }

        public static string ReadSummary(Message message)
        {
            IMessageReader reader = GetReader(message);
            return reader.ReadSummary(message);
        }
    }
}
