using Newtonsoft.Json;
using Worktile.Domain.SocketMessageConverter.Converters;

namespace Worktile.Domain.SocketMessageConverter
{
    static class SocketMessageConverter
    {
        static SocketMessageConverter()
        {
            _converters = new ISocketConverter[]
            {
                new MessageConverter(),
                new OpenConverter()
            };
        }

        static readonly ISocketConverter[] _converters;

        public static (string msg, ISocketConverter converter) Read(string rawMessage)
        {
            foreach (var item in _converters)
            {
                if (item.IsMatch(rawMessage))
                {
                    return (item.Read(rawMessage), item);
                }
            }
            return (null, null);
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
            //ISocketConverter converter = GetConverter(msgType);
            string message = JsonConvert.SerializeObject(data);
            return Process(msgType, message);
        }
    }
}
