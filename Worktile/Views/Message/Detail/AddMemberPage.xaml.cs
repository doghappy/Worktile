using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Common.Extensions;
using Worktile.Enums;
using Worktile.Models.Member;
using Worktile.Models.Message.Session;
using Worktile.Services;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class AddMemberPage : Page, INotifyPropertyChanged
    {
        public AddMemberPage()
        {
            InitializeComponent();
            _members = new List<Member>();
            Members = new ObservableCollection<Member>();
            _messageService = new MessageService();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly List<Member> _members;
        private readonly MessageService _messageService;
        private ChannelSession _session;

        public ObservableCollection<Member> Members { get; }

        private string _queryText;
        public string QueryText
        {
            get => _queryText;
            set
            {
                if (_queryText != value)
                {
                    _queryText = value;
                    OnQueryTextChanged();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QueryText)));
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var detailPage = this.GetParent<ChannelDetailPage>();
            _session = detailPage.Session;
            foreach (var item in DataSource.Team.Members)
            {
                if (item.Role != RoleType.Bot && _session.Members.All(m => m.Uid != item.Uid))
                {
                    item.ForShowAvatar(AvatarSize.X80);
                    _members.Add(item);
                    Members.Add(item);
                }
            }
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var member = btn.DataContext as Member;
            var result = await _messageService.AddMemberToChannelAsync(_session.Id, member.Uid);
            if (result)
            {
                _members.Remove(member);
                Members.Remove(member);
                _session.Members.Add(member);
            }
        }

        private void OnQueryTextChanged()
        {
            Members.Clear();
            string text = QueryText.Trim();
            if (text == string.Empty)
            {
                _members.ForEach(i => Members.Add(i));
            }
            else if (text == ",")
            {
                _members.ForEach(m =>
                {
                    if (m.TethysAvatar.DisplayName.Contains(text, StringComparison.CurrentCultureIgnoreCase) || m.TethysAvatar.DisplayNamePinyin.Contains(text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Members.Add(m);
                    }
                });
            }
        }
    }
}
