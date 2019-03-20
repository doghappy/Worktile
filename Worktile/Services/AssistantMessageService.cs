using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Worktile.ApiModels.Message.ApiPigeonMessages;
using Worktile.Common;
using Worktile.Models.Message;
using Worktile.Models.Message.Session;

namespace Worktile.Services
{
    class AssistantMessageService : MessageService
    {
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

        private string _next;

        protected override string GetMessageUrl(ISession session, int filterType)
        {
            int refType = MessageService.GetRefType(session.PageType);
            var memberSession = session as MemberSession;
            string component = GetComponentByNumber(memberSession.Component.Value);
            string url = $"/api/pigeon/messages?ref_id={memberSession.Id}&ref_type={refType}&filter_type={filterType}&component={component}&size=20";
            if (!string.IsNullOrEmpty(_next))
            {
                url += "&next=" + _next;
            }
            return url;
        }

        protected override IEnumerable<Message> ReadMessages(JToken jToken)
        {
            var apiData = jToken.ToObject<ApiPigeonMessages>();

            foreach (var item in apiData.Data.Messages)
            {
                item.ContentFormat();
                MessageHelper.SetAvatar(item);
                //Messages.Insert(0, item);
                yield return item;
            }
            _next = apiData.Data.Next;
            HasMore = apiData.Data.Next != null;
        }
    }
}
