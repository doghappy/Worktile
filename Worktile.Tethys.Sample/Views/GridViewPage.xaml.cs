using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Tethys.Sample.Models;

namespace Worktile.Tethys.Sample.Views
{
    public sealed partial class GridViewPage : Page, INotifyPropertyChanged
    {
        public GridViewPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Navs = e.Parameter as List<NavItem>;
        }

        List<NavItem> _navs;
        List<NavItem> Navs
        {
            get => _navs;
            set
            {
                if (_navs != value)
                {
                    _navs = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Navs)));
                }
            }
        }


        NavItem _selectedNav;
        NavItem SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (_selectedNav != value)
                {
                    _selectedNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNav)));
                    Frame.Navigate(value.SourcePageType);
                }
            }
        }
    }
}
