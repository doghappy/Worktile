using System.Text.RegularExpressions;

namespace Worktile.Domain.SocketMessageConverter.Converters
{
    class MessageConverter : ISocketConverter
    {
        public MessageConverter()
        {
            _regex = new Regex("^42/message,\\[\"message\",(.+)\\]$");
        }

        const string TEMPLATE = "42/message,[\"message\",$]";
        readonly Regex _regex;

        public bool IsMatch(string rawMessage) => _regex.IsMatch(rawMessage);

        public string Read(string rawMessage)
        {
            var match = _regex.Match(rawMessage);
            return match.Groups[1].Value;
        }

        public string Process(string message) => TEMPLATE.Replace("$", message);
    }
}
