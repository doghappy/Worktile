using System.Text.RegularExpressions;

namespace Worktile.Common
{
    public static class WtMarkdown
    {
        public static string FormatForMessage(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                // @
                msg = Regex.Replace(msg, @"\[@([a-z\d]{32})\|(.+?)\]",
                    match => $"[@{match.Groups[2].Value}](worktile://message/{match.Groups[1].Value})");

                // task
                msg = Regex.Replace(msg, @"\[#([a-z]+)-([a-z\d]{24})\|(.+?)\]",
                    match => $"[{match.Groups[3].Value}](worktile://{match.Groups[1].Value}/{match.Groups[2].Value})");

                // link
                msg = Regex.Replace(msg, @"\[([a-zA-z]+?://[^\s]+?)\|(.*?)\]",
                    match => string.IsNullOrEmpty(match.Groups[2].Value)
                        ? match.Groups[1].Value
                        : $"[{match.Groups[2].Value}]({match.Groups[1].Value})");
            }
            return msg;
        }
    }
}
