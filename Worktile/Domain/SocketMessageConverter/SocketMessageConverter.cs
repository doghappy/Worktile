using Newtonsoft.Json;
using Worktile.Domain.SocketMessageConverter.Converters;

namespace Worktile.Domain.SocketMessageConverter
{
    static class SocketMessageConverter
    {
        static SocketMessageConverter()
        {
            _converters = new[]
            {
                new MessageConverter()
            };
        }

        static readonly ISocketConverter[] _converters;

        public static string Read(string rawMessage)
        {
            foreach (var item in _converters)
            {
                if (item.IsMatch(rawMessage))
                {
                    return item.Read(rawMessage);
                }
            }
            return null;
        }

        public static T Read<T>(string rawMessage) where T : new()
        {
            string msg = Read(rawMessage);
            return JsonConvert.DeserializeObject<T>(msg);
        }

        private static ISocketConverter GetConverter(SocketMessageType msgType)
        {
            ISocketConverter converter = null;
            switch (msgType)
            {
                case SocketMessageType.Message:
                    converter = new MessageConverter();
                    break;
            }
            if (converter == null)
            {
                throw new UnkonwMessageTypeException();
            }
            return converter;
        }

        public static string Process(SocketMessageType msgType, string message)
        {
            ISocketConverter converter = GetConverter(msgType);
            return converter.Process(message);
        }

        public static string Process(SocketMessageType msgType, object data)
        {
            ISocketConverter converter = GetConverter(msgType);
            string message = JsonConvert.SerializeObject(data);
            return converter.Process(message);
        }
    }
}
