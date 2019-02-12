using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.IM;
using Worktile.ViewModels.IM;

namespace Worktile.Views.IM
{
    public sealed partial class ChannelMessagePage : Page, INotifyPropertyChanged
    {
        public ChannelMessagePage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private MessageViewModel _viewModel;
        public MessageViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != value)
                {
                    _viewModel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewModel)));
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var session = e.Parameter as ChatSession;
            ViewModel = MessageViewModel.GetViewModel(session);
        }
    }
}
