using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Message;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message
{
    abstract class DetailViewModel<S> : ViewModel where S : ISession
    {
        public DetailViewModel(S session, Frame contentFrame, MainViewModel mainViewModel)
        {
            Session = session;
            ContentFrame = contentFrame;
            MainViewModel = mainViewModel;
        }

        protected Frame ContentFrame { get; }
        protected MainViewModel MainViewModel { get; }
        public S Session { get; }

        public abstract ObservableCollection<TopNav> Navs { get; protected set; }
        public abstract TopNav SelectedNav { get; set; }

        //private S _session;
        //public S Session
        //{
        //    get => _session;
        //    set
        //    {
        //        if (_session != value)
        //        {
        //            _session = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
    }
}
