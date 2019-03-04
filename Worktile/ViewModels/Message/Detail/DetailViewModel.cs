using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Message;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message
{
    abstract class DetailViewModel : ViewModel
    {
        public DetailViewModel(ISession session, Frame contentFrame, MainViewModel mainViewModel)
        {
            Session = session;
            ContentFrame = contentFrame;
            MainViewModel = mainViewModel;
        }

        protected Frame ContentFrame { get; }
        protected MainViewModel MainViewModel { get; }

        public abstract ObservableCollection<TopNav> Navs { get; protected set; }
        public abstract TopNav SelectedNav { get; set; }

        private ISession _session;
        public ISession Session
        {
            get => _session;
            set
            {
                if (_session != value)
                {
                    _session = value;
                    OnPropertyChanged();
                }
            }
        }

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
        //            OnPropertyChanged();
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
        //            OnPropertyChanged();
        //        }
        //    }
        //}


        //private Member _member;
        //public Member Member
        //{
        //    get => _member;
        //    set
        //    {
        //        if (_member != value)
        //        {
        //            _member = value;
        //            OnPropertyChanged();
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
        //            OnPropertyChanged();
        //        }
        //    }
        //}

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
