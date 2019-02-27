using System.Text.RegularExpressions;
using Worktile.Enums.Message;
using Worktile.Models.Message;

namespace Worktile.Common
{
    public static class WtMarkdown
    {
        public static void ContentFormat(this Message msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Body.Content))
            {
                // @
                msg.Body.Content = Regex.Replace(msg.Body.Content, @"\[@([a-z\d]{32})\|(.+?)\]",
                    match => $"[@{match.Groups[2].Value}](worktile://message/{match.Groups[1].Value})");

                // task
                msg.Body.Content = Regex.Replace(msg.Body.Content, @"\[#([a-z]+)-([a-z\d]{24})\|(.+?)\]",
                    match => $"[{match.Groups[3].Value}](worktile://{match.Groups[1].Value}/{match.Groups[2].Value})");

                // link
                msg.Body.Content = LinkFormat(msg.Body.Content);

                // link without text
                msg.Body.Content = Regex.Replace(msg.Body.Content, @"\[([a-zA-z]+?://[^\s]+?)\]",
                    match => match.Groups[1].Value);
            }

            if (msg.Type == MessageType.Attachment)
            {
                msg.Body.InlineAttachment.Pretext = LinkFormat(msg.Body.InlineAttachment.Pretext);
            }
        }

        private static string LinkFormat(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                text = Regex.Replace(text, @"\[([a-zA-z]+?://[^\s]+?)\|(.*?)\]",
                    match => string.IsNullOrEmpty(match.Groups[2].Value)
                        ? match.Groups[1].Value
                        : $"[{match.Groups[2].Value}]({match.Groups[1].Value})");
            }
            return text;
        }
    }
}
