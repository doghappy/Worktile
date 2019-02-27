using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Worktile.ApiModels;
using Worktile.ApiModels.Message.ApiPigeonMessages;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Models.Message;

namespace Worktile.ViewModels.Message
{
    class AssistantMessageViewModel : MessageViewModel, INotifyPropertyChanged
    {
        public AssistantMessageViewModel(Session session, MainViewModel mainViewModel, TopNav nav) : base(session, mainViewModel)
        {
            _nav = nav;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        TopNav _nav;
        string _next;

        protected override string Url
        {
            get
            {
                string component = null;
                if (Session.Component.HasValue)
                {
                    component = GetComponentByNumber(Session.Component.Value);
                }
                string url = $"/api/pigeon/messages?ref_id={Session.Id}&ref_type={Session.RefType}&filter_type={_nav.FilterType}&component={component}&size=20";
                if (!string.IsNullOrEmpty(_next))
                {
                    url += "&next=" + _next;
                }
                return url;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        protected override void ReadMessage(JToken jToken)
        {
            var apiData = jToken.ToObject<ApiPigeonMessages>();

            foreach (var item in apiData.Data.Messages)
            {
                Messages.Insert(0, new ViewMessage
                {
                    Id = item.Id,
                    Avatar = new TethysAvatar
                    {
                        DisplayName = item.From.DisplayName,
                        Source = AvatarHelper.GetAvatarBitmap(item.From.Avatar, AvatarSize.X80, item.From.Type),
                        Background = AvatarHelper.GetColorBrush(item.From.DisplayName)
                    },
                    Content = MessageHelper.GetContent(item),
                    Time = item.CreatedAt,
                    Type = item.Type,
                    IsPinned = item.IsPending
                });
            }
            _next = apiData.Data.Next;
            HasMore = apiData.Data.Next != null;
        }

        public async override Task PinMessageAsync(ViewMessage msg)
        {
            string url = $"/api/unreads/{Session.Id}/messages/{msg.Id}/pending";
            var client = new WtHttpClient();
            var response = await client.PostAsync<ApiDataResponse<bool>>(url);
            if (response.Code == 200 && response.Data)
            {
                msg.IsPinned = true;
            }
        }

        public async override Task UnPinMessageAsync(ViewMessage msg)
        {
            string url = $"/api/unreads/{Session.Id}/messages/{msg.Id}/pending";
            var client = new WtHttpClient();
            var response = await client.DeleteAsync<ApiDataResponse<bool>>(url);
            if (response.Code == 200 && response.Data)
            {
                msg.IsPinned = false;
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
    }
}
