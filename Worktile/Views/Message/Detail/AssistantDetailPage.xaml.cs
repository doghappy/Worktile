using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.Message.NavigationParam;
using Worktile.ViewModels.Message;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class AssistantDetailPage : Page, INotifyPropertyChanged
    {
        public AssistantDetailPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        ToMessageDetailPageParam _navParam;

        private DetailViewModel _viewModel;
        private DetailViewModel ViewModel
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
            _navParam = e.Parameter as ToMessageDetailPageParam;
            ViewModel = new AssistantDetailViewModel(_navParam.Session, ContentFrame, _navParam.MainViewModel);
        }
    }
}
