using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Worktile.ApiModels.Message.ApiPigeonMessages;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Models.Message;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail.Content
{
    class AssistantMessageViewModel : BaseSessionMessageViewModel<MemberSession>, INotifyPropertyChanged
    {
        public AssistantMessageViewModel(MemberSession session, TopNav nav)
            : base(session)
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
                string component = GetComponentByNumber(Session.Component.Value);
                string url = $"/api/pigeon/messages?ref_id={Session.Id}&ref_type={RefType}&filter_type={_nav.FilterType}&component={component}&size=20";
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
                item.ContentFormat();
                MessageHelper.SetAvatar(item);
                Messages.Insert(0, item);
            }
            _next = apiData.Data.Next;
            HasMore = apiData.Data.Next != null;
        }

        public async override Task<bool> PinAsync(Models.Message.Message msg)
        {
            bool result = await base.PinAsync(msg);
            if (result)
            {
                msg.IsPending = true;
            }
            return result;
        }

        public async override Task<bool> UnPinAsync(Models.Message.Message msg)
        {
            bool result = await base.UnPinAsync(msg);
            if (result)
            {
                msg.IsPending = false;
            }
            return result;
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
