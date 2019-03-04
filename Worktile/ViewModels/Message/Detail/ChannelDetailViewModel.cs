using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Message;
using Worktile.Models.Message.NavigationParam;
using Worktile.Models.Message.Session;
using Worktile.Views.Message.Detail.Content;
using Worktile.Views.Message.Detail.Content.Pin;

namespace Worktile.ViewModels.Message
{
    class ChannelDetailViewModel : SessionDetailViewModel, INotifyPropertyChanged
    {
        public ChannelDetailViewModel(ISession session, Frame contentFrame, MainViewModel mainViewModel)
            : base(session, contentFrame, mainViewModel) { }

        public event PropertyChangedEventHandler PropertyChanged;

        public override ObservableCollection<TopNav> Navs { get; protected set; }

        public override string PaneTitle => "群组成员";

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
                            ContentFrame.Navigate(typeof(ChannelMessagePage), new ToUnReadMsgPageParam
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
                            ContentFrame.Navigate(typeof(ChannelPinnedPage), Session);
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        //private readonly Frame _contentFrame;
        //private readonly MainViewModel _mainViewModel;

        //private ISession _session;
        //public ISession Session
        //{
        //    get => _session;
        //    set
        //    {
        //        if (_session != value)
        //        {
        //            _session = value;
        //            LoadTopNavs(value);
        //            SelectedNav = Navs.First();
        //            SetPaneTitle(value);
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Session)));
        //        }
        //    }
        //}

        //private TopNav _selectedNav;
        //public TopNav SelectedNav
        //{
        //    get => _selectedNav;
        //    set
        //    {
        //        if (_selectedNav != value)
        //        {
        //            _selectedNav = value;
        //            int index = Navs.IndexOf(value);
        //            if (value.Name == "未读" || value.Name == "已读" || value.Name == "待处理")
        //            {
        //                _contentFrame.Navigate(typeof(AssistantMessagePage), new ToUnReadMsgPageParam
        //                {
        //                    Session = Session,
        //                    Nav = value,
        //                    MainViewModel = _mainViewModel
        //                });
        //            }
        //            else if (value.Name == "消息")
        //            {
        //                _contentFrame.Navigate(typeof(SessionMessagePage), new ToUnReadMsgPageParam
        //                {
        //                    Session = Session,
        //                    Nav = value,
        //                    MainViewModel = _mainViewModel
        //                });
        //            }
        //            else if (value.Name == "文件")
        //            {
        //                _contentFrame.Navigate(typeof(FilePage), Session);
        //            }
        //            else if (value.Name == "固定消息")
        //            {
        //                _contentFrame.Navigate(typeof(PinnedPage), Session);
        //            }
        //            else if (value.Name == "已读")
        //            {

        //            }
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNav)));
        //        }
        //    }
        //}

        //private bool _isPaneOpen;
        //public bool IsPaneOpen
        //{
        //    get => _isPaneOpen;
        //    set
        //    {
        //        if (_isPaneOpen != value)
        //        {
        //            _isPaneOpen = value;
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPaneOpen)));
        //        }
        //    }
        //}

        //public ObservableCollection<TopNav> Navs { get; }

        //private Member _member;
        //public Member Member
        //{
        //    get => _member;
        //    set
        //    {
        //        if (_member != value)
        //        {
        //            _member = value;
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
        //        }
        //    }
        //}

        //private string _paneTitle;
        //public string PaneTitle
        //{
        //    get => _paneTitle;
        //    set
        //    {
        //        if (_paneTitle != value)
        //        {
        //            _paneTitle = value;
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaneTitle)));
        //        }
        //    }
        //}

        //public override ObservableCollection<TopNav> Navs => throw new System.NotImplementedException();



        //private void SetPaneTitle(ISession session)
        //{
        //    if (session.GetType() == typeof(MemberSession))
        //    {
        //        PaneTitle = "成员";
        //    }
        //    else
        //    {
        //        PaneTitle = "群组成员";
        //    }
        //}

        //private void LoadTopNavs(ISession session)
        //{
        //    if (session.GetType() == typeof(MemberSession))
        //    {
        //        Navs.Add(new TopNav { Name = "消息" });
        //        Navs.Add(new TopNav { Name = "文件" });
        //        Navs.Add(new TopNav { Name = "固定消息", IsPin = true });
        //    }
        //    else
        //    {
        //        Navs.Add(new TopNav { Name = "未读", FilterType = 2 });
        //        Navs.Add(new TopNav { Name = "已读", FilterType = 4 });
        //        Navs.Add(new TopNav { Name = "待处理", FilterType = 3 });
        //    }
        //}

        //public async Task LoadMemberInfoAsync()
        //{
        //    if (Member == null)
        //    {
        //        IsActive = true;
        //        string url = $"/api/users/{Session}/basic";
        //        var client = new WtHttpClient();
        //        var data = await client.GetAsync<ApiDataResponse<Member>>(url);
        //        Member = data.Data;
        //        IsActive = false;
        //    }
        //}
    }
}
