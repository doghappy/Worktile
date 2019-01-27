using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Worktile.ApiModels.IM.ApiPigeonMessages;
using Worktile.Enums;
using Worktile.Models.IM;
using Worktile.ViewModels.Infrastructure;

namespace Worktile.ViewModels.IM
{
    public class SessionMessageViewModel : MessageViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected override string MessageUrl
        {
            get
            {
                int refType = Session.Type == ChatType.Channel ? 1 : 2;
                string component = null;
                if (Session.Component.HasValue)
                {
                    component = GetComponentByNumber(Session.Component.Value);
                }
                string uri = $"/api/pigeon/messages?ref_id={Session.Id}&ref_type={refType}&filter_type={SelectedNav.FilterType}&component={component}&size=20";
                if (!string.IsNullOrEmpty(SelectedNav.Next))
                {
                    uri += "&next=" + SelectedNav.Next;
                }
                return uri;
            }
        }

        private string GetComponentByNumber(int c)
        {
            string component = null;
            switch (c)
            {
                case 0: component = "message"; break;
                case 1: component = "drive"; break;
                case 2: component = "task"; break;
                case 3: component = "calendar"; break;
                case 4: component = "report"; break;
                case 5: component = "crm"; break;
                case 6: component = "approval"; break;
                case 9: component = "leave"; break;
                case 20: component = "bulletin"; break;
                case 30: component = "appraisal"; break;
                case 40: component = "okr"; break;
                case 50: component = "portal"; break;
                case 60: component = "mission"; break;
            }
            return component;
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        protected override void ReadApiData(JToken jToken)
        {
            var apiData = jToken.ToObject<ApiPigeonMessages>();
            foreach (var item in apiData.Data.Messages)
            {
                item.From.Avatar = AvatarHelper.GetAvatarUrl(item.From.Avatar, AvatarSize.X80, item.From.Type);
                item.From.Background = AvatarHelper.GetColor(item.From.DisplayName);
                item.From.Initials = AvatarHelper.GetInitials(item.From.DisplayName);
                SelectedNav.Messages.Insert(0, item);
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


            SelectedNav.Next = apiData.Data.Next;
            SelectedNav.HasMore = apiData.Data.Next != null;
        }
    }
}
