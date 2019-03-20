using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels;
using Worktile.Common;
using Worktile.Common.Communication;
using Worktile.Common.Extensions;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Message.Session;
using Worktile.Services;

namespace Worktile.Views.Message.Dialog
{
    public sealed partial class FileShareDialg : ContentDialog, INotifyPropertyChanged
    {
        public FileShareDialg()
        {
            InitializeComponent();
            Avatars = new ObservableCollection<GroupWrapper>();
            _messageService = new MessageService();
            _entityService = new EntityService();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly MessageService _messageService;
        readonly EntityService _entityService;

        public string FileId { get; set; }
        public MasterPage MasterPage { get; set; }

        ObservableCollection<GroupWrapper> Avatars { get; }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }

        private TethysAvatar _selectedItem;
        public TethysAvatar SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    IsPrimaryButtonEnabled = value != null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
                }
            }
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var avatars = new List<TethysAvatar>();
            avatars.AddRange(DataSource.Team.Members.Where(m => m.IsTrueMember()).Select(m => m.TethysAvatar));
            avatars.AddRange(MasterPage.Sessions.Where(s => s is ChannelSession).Select(c => c.TethysAvatar));

            var group = avatars
                .GroupBy(a => a.DisplayNamePinyin[0].ToString().ToUpper())
                .Select(a => new GroupWrapper(a.OrderBy(i => i.DisplayNamePinyin))
                {
                    Key = a.Key
                })
                .OrderBy(g => g.Key);
            foreach (var item in group)
            {
                Avatars.Add(item);
            }
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (SelectedItem != null)
            {
                string sessionId = null;
                int refType = 0;
                var memberSessions = MasterPage.Sessions
                    .Where(s => s is MemberSession)
                    .Select(s => s as MemberSession)
                    .ToList();
                if (SelectedItem.Id.Length == 32)
                {
                    var session = memberSessions.SingleOrDefault(s => s.To.Uid == SelectedItem.Id);
                    if (session == null)
                    {
                        session = await _messageService.CreateSessionAsync(SelectedItem.Id);
                        session.ForShowAvatar(AvatarSize.X80);
                        MasterPage.InserSession(session, false);
                    }
                    refType = 2;
                    sessionId = session.Id;
                }
                else if(SelectedItem.Id.Length == 24)
                {
                    refType = 1;
                }
                await _entityService.ShareAsync(FileId, sessionId, refType);
            }
        }
    }
}
