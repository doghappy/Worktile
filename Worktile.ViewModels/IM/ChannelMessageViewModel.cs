using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Worktile.ApiModels.IM.ApiMessages;
using Worktile.Models.IM;
using System.Linq;
using Worktile.Models.IM.Message;
using Worktile.ViewModels.Infrastructure;

namespace Worktile.ViewModels.IM
{
    public class ChannelMessageViewModel : MessageViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected override string MessageUrl
        {
            get
            {
                int refType = Session.Type == ChatType.Channel ? 1 : 2;
                return $"/api/messages?ref_id={Session.Id}&ref_type={refType}&latest_id={SelectedNav.LatestId}&size=20";
            }
        }

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
                item.From.Avatar = AvatarHelper.GetAvatarUrl(item.From.Avatar, 80);
                item.From.Background = AvatarHelper.GetColor(item.From.DisplayName);
                item.From.Initials = AvatarHelper.GetInitials(item.From.DisplayName);

                //string key = $"{item.CreatedAt.ToString("m")} {CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(item.CreatedAt.DayOfWeek)}";
                //var groupItem = SelectedNav.MessageGroup.SingleOrDefault(i => i.Key == key);
                //if (groupItem == null)
                //{
                //    groupItem = new MessageGroup
                //    {
                //        Key = key
                //    };
                //    SelectedNav.MessageGroup.Insert(0, groupItem);
                //}
                //groupItem.Messages.Insert(0, item);
                if (flag)
                    SelectedNav.Messages.Insert(0, item);
                else
                    SelectedNav.Messages.Add(item);
            }
            SelectedNav.LatestId = apiData.Data.LatestId;
            SelectedNav.HasMore = apiData.Data.More;
        }
    }
}
