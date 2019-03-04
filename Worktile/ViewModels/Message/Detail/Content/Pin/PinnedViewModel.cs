using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Worktile.ApiModels;
using Worktile.ApiModels.ApiPinnedMessages;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail.Content.Pin
{
    abstract class PinnedViewModel<S> : MessageBaseViewModel<S> where S : ISession
    {
        public PinnedViewModel(S session) : base(session)
        {
            Messages = new IncrementalCollection<Models.Message.Message>(LoadMessagesAsync);
        }

        private string _anchor;

        public IncrementalCollection<Models.Message.Message> Messages { get; }

        private async Task<IEnumerable<Models.Message.Message>> LoadMessagesAsync()
        {
            IsActive = true;
            const int SIZE = 10;
            var list = new List<Models.Message.Message>();
            string url = $"/api/pinneds?{IdType}={Session.Id}&anchor={_anchor}&size={SIZE}";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiPinnedMessages>(url);
            if (data.Code == 200 && data.Data.Pinneds.Any())
            {
                _anchor = data.Data.Pinneds.Last().Id;
                if (data.Data.Batch < SIZE)
                {
                    Messages.HasMoreItems = false;
                }
                foreach (var item in data.Data.Pinneds)
                {
                    IMemberBase member = null;
                    if (item.Reference.From.Type == FromType.User)
                        member = DataSource.Team.Members.Single(m => m.Uid == item.Reference.From.Uid);
                    else if (item.Reference.From.Type == FromType.Service)
                        member = DataSource.Team.Services.Single(m => m.ServiceId == item.Reference.From.Uid);

                    item.Reference.From.TethysAvatar = new TethysAvatar
                    {
                        DisplayName = member.DisplayName,
                        Source = AvatarHelper.GetAvatarBitmap(member.Avatar, AvatarSize.X80, item.Reference.From.Type)
                    };
                    if (Path.GetExtension(member.Avatar).ToLower() == ".png")
                        item.Reference.From.TethysAvatar.Background = new SolidColorBrush(Colors.White);
                    else
                        item.Reference.From.TethysAvatar.Background = AvatarHelper.GetColorBrush(member.DisplayName);
                    list.Add(item.Reference);
                }
            }
            else
            {
                Messages.HasMoreItems = false;
            }
            IsActive = false;
            return list;
        }

        public async Task UnPinAsync(Models.Message.Message msg)
        {
            string url = $"/api/messages/{msg.Id}/unpinned?{IdType}={Session.Id}";
            var client = new WtHttpClient();
            var response = await client.DeleteAsync<ApiDataResponse<bool>>(url);
            if (response.Code == 200 && response.Data)
            {
                Messages.Remove(msg);
            }
        }
    }
}
