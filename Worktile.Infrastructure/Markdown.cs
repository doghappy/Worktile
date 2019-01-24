using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Worktile.Infrastructure
{
    public static class Markdown
    {
        public static string FormatForMessage(string msg)
        {
            // @
            msg = Regex.Replace(msg, @"\[@([a-z\d]{32})\|(.+?)\]",
                match => $"[@{match.Groups[2].Value}](worktile://message/{match.Groups[1].Value})");

            // task
            msg = Regex.Replace(msg, @"\[#([a-z]+)-([a-z\d]{24})\|(.+?)\]",
                match => $"[{match.Groups[3].Value}](worktile://{match.Groups[1].Value}/{match.Groups[2].Value})");
            return msg;
        }
    }
}
