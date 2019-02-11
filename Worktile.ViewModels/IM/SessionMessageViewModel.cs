﻿using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Worktile.ApiModels.IM.ApiMessages;
using Worktile.Enums;
using Worktile.Infrastructure;
using Worktile.ViewModels.Infrastructure;

namespace Worktile.ViewModels.IM
{
    public class SessionMessageViewModel : MessageViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        protected override void ReadApiData(JToken jToken)
        {
            var apiData = jToken.ToObject<ApiMessages>();
            bool flag = false;
            if (SelectedNav.Messages.Any())
            {
                apiData.Data.Messages.Reverse();
                flag = true;
            }
            foreach (var item in apiData.Data.Messages)
            {
                item.From.Avatar = AvatarHelper.GetAvatarUrl(item.From.Avatar, AvatarSize.X80, item.From.Type);
                item.From.Background = AvatarHelper.GetColor(item.From.DisplayName);
                item.From.Initials = AvatarHelper.GetInitials(item.From.DisplayName);
                item.Body.Content = Markdown.FormatForMessage(item.Body.Content);
                //SelectedNav.Messages.Add(item);
                if (flag)
                    SelectedNav.Messages.Insert(0, item);
                else
                    SelectedNav.Messages.Add(item);
            }
            //foreach (var item in apiData.Data.Messages)
            //{
            //    item.From.Avatar = AvatarHelper.GetAvatarUrl(item.From.Avatar, 80);
            //    item.From.Background = AvatarHelper.GetColor(item.From.DisplayName);
            //    item.From.Initials = AvatarHelper.GetInitials(item.From.DisplayName);

            //    string key = $"{item.CreatedAt.ToString("m")} {CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(item.CreatedAt.DayOfWeek)}";
            //    var groupItem = SelectedNav.MessageGroup.SingleOrDefault(i => i.Key == key);
            //    if (groupItem == null)
            //    {
            //        groupItem = new MessageGroup
            //        {
            //            Key = key
            //        };
            //        SelectedNav.MessageGroup.Insert(0, groupItem);
            //    }
            //    groupItem.Messages.Insert(0, item);
            //}
            SelectedNav.LatestId = apiData.Data.LatestId;
            SelectedNav.HasMore = apiData.Data.More;
        }
    }
}
