using System;
using Worktile.Enums.Message;

namespace Worktile.Models.Message.Session
{
    public interface ISession
    {
        string Id { get; }
        string NamePinyin { get; }
        TethysAvatar TethysAvatar { get; }
        bool Starred { get; set; }
        DateTime LatestMessageAt { get; }
        int Show { get; }
        int UnRead { get; set; }
        DateTime CreatedAt { get; }
        string TeamId { get; }
        PageType PageType { get; }
    }
}
