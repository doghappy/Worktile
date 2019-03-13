using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.Common.Extensions;
using Worktile.Common.WtRequestClient;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail
{
    class JoinChannelViewModel : BaseJoinChatViewModel, INotifyPropertyChanged
    {
        public JoinChannelViewModel(MasterViewModel masterViewModel) : base(masterViewModel)
        {
            Sessions = new ObservableCollection<ChannelSession>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        List<ChannelSession> _sessions;

        public ObservableCollection<ChannelSession> Sessions { get; }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task LoadDataAsync()
        {
            IsActive = true;
            string url = "api/channels?type=all&filter=all&status=ok";
            var data = await WtHttpClient.GetAsync<ApiDataResponse<List<ChannelSession>>>(url);
            _sessions = data.Data;
            foreach (var item in data.Data)
            {
                item.ForShowAvatar();
                Sessions.Add(item);
            }
            IsActive = false;
        }

        protected override void OnQueryTextChanged()
        {
            Sessions.Clear();
            string text = QueryText.Trim();
            if (text == string.Empty)
            {
                foreach (var item in _sessions)
                {
                    Sessions.Add(item);
                }
            }
            else if (text != ",")
            {
                foreach (var item in _sessions)
                {
                    if (item.Name.Contains(text, StringComparison.CurrentCultureIgnoreCase) || item.NamePinyin.Contains(text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Sessions.Add(item);
                    }
                }
            }
        }

        public async Task<bool> ActiveAsync(ChannelSession session)
        {
            string url = $"api/channels/{session.Id}/active";
            var data = await WtHttpClient.PutAsync<ApiResponse>(url);
            if (data.Code == 200)
            {
                CreateNewSession(session);
                return true;
            }
            return false;
        }

        public async Task<bool> JoinAsync(ChannelSession session)
        {
            string url = $"api/channels/{session.Id}/join";
            var data = await WtHttpClient.PutAsync<ApiDataResponse<bool>>(url);
            if (data.Code == 200 && data.Data)
            {
                MasterViewModel.Sessions.Insert(0, session);
                MasterViewModel.SelectedSession = session;
                return true;
            }
            return false;
        }
    }
}
