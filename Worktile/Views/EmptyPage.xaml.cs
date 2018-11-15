using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Worktile.Views
{
    public sealed partial class EmptyPage : Page, INotifyPropertyChanged
    {
        public EmptyPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _emptyContent;
        public string EmptyContent
        {
            get => _emptyContent;
            set
            {
                _emptyContent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmptyContent)));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            EmptyContent = e.Parameter.ToString();
        }
    }
}
