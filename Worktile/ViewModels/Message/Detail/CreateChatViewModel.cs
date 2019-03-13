using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.Common;
using Worktile.Common.Extensions;
using Worktile.Common.WtRequestClient;
using Worktile.Models;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail
{
    class CreateChatViewModel : BaseJoinChatViewModel, INotifyPropertyChanged
    {
        public CreateChatViewModel(MasterViewModel masterViewModel) : base(masterViewModel)
        {
            Avatars = new ObservableCollection<TethysAvatar>();
            foreach (var item in DataSource.Team.Members)
            {
                if (item.IsTrueMember())
                {
                    Avatars.Add(item.TethysAvatar);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TethysAvatar> Avatars { get; }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        protected override void OnQueryTextChanged()
        {
            Avatars.Clear();
            string text = QueryText.Trim();
            if (text == string.Empty)
            {
                foreach (var item in DataSource.Team.Members)
                {
                    if (item.IsTrueMember())
                    {
                        Avatars.Add(item.TethysAvatar);
                    }
                }
            }
            else if (text != ",")
            {
                foreach (var item in DataSource.Team.Members)
                {
                    if (item.IsTrueMember() && (item.TethysAvatar.Name.Contains(text, StringComparison.CurrentCultureIgnoreCase) || item.DisplayNamePinyin.Contains(text, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        Avatars.Add(item.TethysAvatar);
                    }
                }
            }
        }

        public async Task CreateAsync(TethysAvatar avatar)
        {
            var data = await WtHttpClient.PostAsync<ApiDataResponse<MemberSession>>("/api/session", new { uid = avatar.Id });
            if (data.Code == 200)
            {
                CreateNewSession(data.Data);
            }
        }
    }
}
