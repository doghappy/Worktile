using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Message;
using Worktile.Models.Member;
using Worktile.Models.Message.NavigationParam;
using Worktile.Models.Message.Session;
using Worktile.Views.Message.Detail.Content;
using Worktile.Views.Message.Detail.Content.Pin;
using System.Threading.Tasks;
using Worktile.Common.WtRequestClient;
using Worktile.ApiModels;

namespace Worktile.ViewModels.Message
{
    class MemberDetailViewModel : SessionDetailViewModel<MemberSession>, INotifyPropertyChanged
    {
        public MemberDetailViewModel(MemberSession session, Frame contentFrame, MainViewModel mainViewModel)
            : base(session, contentFrame, mainViewModel) { }

        public event PropertyChangedEventHandler PropertyChanged;

        public override ObservableCollection<TopNav> Navs { get; protected set; }

        public override string PaneTitle => "成员";

        private TopNav _selectedNav;
        public override TopNav SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (_selectedNav != value)
                {
                    _selectedNav = value;
                    int index = Navs.IndexOf(value);
                    switch (index)
                    {
                        case 0:
                            ContentFrame.Navigate(typeof(MemberMessagePage), new ToUnReadMsgPageParam
                            {
                                Session = Session,
                                Nav = value,
                                MainViewModel = MainViewModel
                            });
                            break;
                        case 1:
                            ContentFrame.Navigate(typeof(FilePage), Session);
                            break;
                        case 2:
                            ContentFrame.Navigate(typeof(MemberPinnedPage), Session);
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private Member _member;
        public Member Member
        {
            get => _member;
            set
            {
                if (_member != value)
                {
                    _member = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task LoadMemberInfoAsync()
        {
            if (Member == null)
            {
                IsActive = true;
                string url = $"/api/users/{Session.To.Uid}/basic";
                var data = await WtHttpClient.GetAsync<ApiDataResponse<Member>>(url);
                Member = data.Data;
                IsActive = false;
            }
        }
    }
}
