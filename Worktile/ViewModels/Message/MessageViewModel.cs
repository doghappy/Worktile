using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Message;

namespace Worktile.ViewModels.Message
{
    abstract class MessageViewModel : ViewModel
    {
        public MessageViewModel(Session session)
        {
            Session = session;
            Messages = new ObservableCollection<ViewMessage>();
        }

        protected Session Session { get; }
        protected abstract string Url { get; }
        public ObservableCollection<ViewMessage> Messages { get; }
        public bool? HasMore { get; protected set; }

        protected abstract void ReadMessage(JToken jToken);
        public abstract Task PinMessageAsync(ViewMessage msg);
        public abstract Task UnPinMessageAsync(ViewMessage msg);

        public async Task LoadMessagesAsync()
        {
            IsActive = true;
            var client = new WtHttpClient();
            var data = await client.GetJTokenAsync(Url);
            ReadMessage(data);
            IsActive = false;
        }

        public async Task ClearUnReadAsync(MainViewModel mainViewModel)
        {
            string url = $"/api/messages/unread/clear?ref_id={Session.Id}";
            var client = new WtHttpClient();
            await client.PutAsync<object>(url);
            mainViewModel.UnreadBadge -= Session.UnRead;
            await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() => Session.UnRead = 0));
        }

        public async void OnMessageReceived(Models.Message.Message apiMsg)
        {
            if (apiMsg.To.Id == Session.Id)
            {
                await Task.Run(async () =>
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        var member = DataSource.Team.Members.Single(m => m.Uid == apiMsg.From.Uid);
                        Messages.Add(new ViewMessage
                        {
                            Id = apiMsg.Id,
                            Avatar = new TethysAvatar
                            {
                                DisplayName = member.DisplayName,
                                Source = AvatarHelper.GetAvatarBitmap(member.Avatar, AvatarSize.X80, FromType.User),
                                Foreground = new SolidColorBrush(Colors.White),
                                Background = AvatarHelper.GetColorBrush(member.DisplayName)
                            },
                            Content = MessageHelper.GetContent(apiMsg),
                            Time = apiMsg.CreatedAt,
                            IsPinned = false,
                            Type = apiMsg.Type
                        });
                    });
                });
            }
        }
    }
}
