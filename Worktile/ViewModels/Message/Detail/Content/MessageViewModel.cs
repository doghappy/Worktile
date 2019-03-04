using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail.Content
{
    abstract class MessageViewModel<S> : ViewModel where S : ISession
    {
        public MessageViewModel(S session, MainViewModel mainViewModel)
        {
            Session = session;
            MainViewModel = mainViewModel;
            Messages = new ObservableCollection<Models.Message.Message>();
        }

        protected S Session { get; }
        protected abstract string Url { get; }
        public ObservableCollection<Models.Message.Message> Messages { get; }
        public bool? HasMore { get; protected set; }
        protected MainViewModel MainViewModel { get; }
        protected int RefType => Session.PageType == PageType.Channel ? 1 : 2;

        protected abstract void ReadMessage(JToken jToken);
        public abstract Task PinMessageAsync(Models.Message.Message msg);
        public abstract Task UnPinMessageAsync(Models.Message.Message msg);

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
