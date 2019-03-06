using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Worktile.Common.WtRequestClient;
using Worktile.Models.Message.Session;
using Worktile.Enums.Message;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.Helpers;
using Worktile.Common;
using Worktile.Models;
using System.Linq;
using Worktile.Enums;
using Newtonsoft.Json;
using Worktile.ApiModels;
using Worktile.Models.Message;

namespace Worktile.ViewModels.Message.Detail.Content
{
    abstract class BaseSessionMessageViewModel<S> : MessageViewModel<S> where S : ISession
    {
        public BaseSessionMessageViewModel(S session, MainViewModel mainViewModel) : base(session)
        {
            MainViewModel = mainViewModel;
            Messages = new ObservableCollection<Models.Message.Message>();
        }

        protected abstract string Url { get; }
        public ObservableCollection<Models.Message.Message> Messages { get; }
        public bool? HasMore { get; protected set; }
        protected MainViewModel MainViewModel { get; }

        protected abstract void ReadMessage(JToken jToken);

        public async Task LoadMessagesAsync()
        {
            IsActive = true;
            var client = new WtHttpClient();
            var data = await client.GetJTokenAsync(Url);
            ReadMessage(data);
            IsActive = false;
        }

        public async Task ClearUnReadAsync()
        {
            string url = $"/api/messages/unread/clear?ref_id={Session.Id}";
            var client = new WtHttpClient();
            await client.PutAsync<object>(url);
            MainViewModel.UnreadBadge -= Session.UnRead;
            await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() => Session.UnRead = 0));
        }

        public async virtual Task<bool> PinAsync(Models.Message.Message msg)
        {
            string url = "/api/pinneds";
            var client = new WtHttpClient();
            var req = new
            {
                type = 1,
                message_id = msg.Id,
                worktile = Session.Id
            };
            string json = JsonConvert.SerializeObject(req);
            json = json.Replace("worktile", IdType);
            var response = await client.PostAsync<ApiResponse>(url, json);
            if (response.Code == 200)
            {
                return true;
            }
            return false;
        }

        public async void OnMessageReceived(Models.Message.Message message)
        {
            if (message.To.Id == Session.Id)
            {
                await Task.Run(async () =>
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        var member = DataSource.Team.Members.Single(m => m.Uid == message.From.Uid);
                        message.From.TethysAvatar = new TethysAvatar
                        {
                            DisplayName = member.DisplayName,
                            Source = AvatarHelper.GetAvatarBitmap(member.Avatar, AvatarSize.X80, FromType.User),
                            Background = AvatarHelper.GetColorBrush(member.DisplayName)
                        };
                        message.IsPinned = false;
                        Messages.Add(message);
                    });
                });
            }
        }
    }
}
