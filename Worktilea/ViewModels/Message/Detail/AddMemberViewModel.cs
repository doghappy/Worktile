using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Worktile.Models.Member;
using System.Linq;
using System;
using System.Threading.Tasks;
using Worktile.Models.Message.Session;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Common.Extensions;
using Worktile.Common.Communication;
using Worktile.ApiModels;

namespace Worktile.ViewModels.Message.Detail
{
    class AddMemberViewModel : ViewModel, INotifyPropertyChanged
    {
        public AddMemberViewModel(ChannelSession session)
        {
            Members = new ObservableCollection<Member>();
            _session = session;
            _members = new List<Member>();
            foreach (var item in DataSource.Team.Members)
            {
                if (item.Role != RoleType.Bot && session.Members.All(m => m.Uid != item.Uid))
                {
                    item.ForShowAvatar(AvatarSize.X80);
                    _members.Add(item);
                    Members.Add(item);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly List<Member> _members;
        private readonly ChannelSession _session;

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
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
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

        public async Task AddMemberAsync(Member member)
        {
            string url = $"api/channels/{_session.Id}/invite";
            var req = new { uid = member.Uid };
            var res = await WtHttpClient.PutAsync<ApiDataResponse<bool>>(url, req);
            if (res.Code == 200 && res.Data)
            {
                _members.Remove(member);
                Members.Remove(member);
                _session.Members.Add(member);
            }
        }
    }
}
