using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Worktile.Domain.SocketMessageConverter.Converters
{
    class OpenConverter : ISocketConverter
    {
        public OpenConverter()
        {
            _regex = new Regex("^0{\"sid\":\"{.*\\\\\"id\\\\\":\\\\\"(\\w+)\\\\\".*}$");
        }

        readonly Regex _regex;

        public bool IsMatch(string rawMessage) => _regex.IsMatch(rawMessage);

        public string Read(string rawMessage)
        {
            var match = _regex.Match(rawMessage);
            return match.Groups[1].Value;
        }

        public string Process(string message) => throw new NotImplementedException();
    }
}
