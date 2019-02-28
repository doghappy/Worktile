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
                msg.Body.Content = AtFormat(msg.Body.Content);

                // task
                msg.Body.Content = WtLinkFormat(msg.Body.Content);

                // task/project
                msg.Body.Content = Regex.Replace(msg.Body.Content, @"\[/tasks/projects/([a-z\d]{24})\|(.+?)\]",
                    match => $"[{match.Groups[2].Value}](worktile://task/project/{match.Groups[1].Value})");

                //[/drive/5850c79484279c729a2eaed5|法律文件]
                msg.Body.Content = Regex.Replace(msg.Body.Content, @"\[/drive/([a-z\d]{24})\|(.+?)\]",
                    match => $"[{match.Groups[2].Value}](worktile://drive/{match.Groups[1].Value})");

                // link
                msg.Body.Content = LinkFormat(msg.Body.Content);

                // link without text
                msg.Body.Content = Regex.Replace(msg.Body.Content, @"\[([a-zA-z]+?://[^\s]+?)\]",
                    match => match.Groups[1].Value);
            }

            switch (msg.Type)
            {
                case MessageType.Attachment:
                    {
                        msg.Body.InlineAttachment.Pretext = LinkFormat(msg.Body.InlineAttachment.Pretext);
                        msg.Body.InlineAttachment.Text = LinkFormat(msg.Body.InlineAttachment.Text);
                        foreach (var item in msg.Body.InlineAttachment.Fields)
                        {
                            if (!string.IsNullOrWhiteSpace(item.Value))
                            {
                                item.Value = Regex.Replace(item.Value, @"\[([a-zA-z]+?://[^\s]+?)\|(.*?)\]",
                                    match => match.Groups[2].Value);
                            }
                        }
                    }
                    break;
                case MessageType.LeaveApplication:
                case MessageType.CrmContract:
                    {
                        msg.Body.InlineAttachment.Text = AtFormat(msg.Body.InlineAttachment.Text);
                        msg.Body.InlineAttachment.Text = WtLinkFormat(msg.Body.InlineAttachment.Text);
                    }
                    break;
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

        private static string AtFormat(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                text = Regex.Replace(text, @"\[@([a-z\d]{32})\|(.+?)\]",
                    match => $"[@{match.Groups[2].Value}](worktile://message/{match.Groups[1].Value})");
            }
            return text;
        }

        private static string WtLinkFormat(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                text = Regex.Replace(text, @"\[#([a-z]+)-([a-z\d]{24})\|(.+?)\]",
                    match => $"[{match.Groups[3].Value}](worktile://{match.Groups[1].Value}/{match.Groups[2].Value})");
            }
            return text;
        }
    }
}
