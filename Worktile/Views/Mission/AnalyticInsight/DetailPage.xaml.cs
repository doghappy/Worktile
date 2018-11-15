using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Worktile.Views.Mission.AnalyticInsight
{
    public sealed partial class DetailPage : Page, INotifyPropertyChanged
    {
        public DetailPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private TopNavItem _nav;
        private TopNavItem _subNav;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dynamic param = e.Parameter;
            _nav = param.Nav as TopNavItem;
            _subNav = param.SubNav as TopNavItem;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public async Task RequestApiMissionVnextWorkAnalyticInsightIdSidContent()
        {
            //string uri = $"/api/mission-vnext/work/analytic-insight/{_nav.Id}/{_nav.s}/content?fpids=&fuids=&from=&to=&pi=0&ps=20";
        }

        //private ObservableCollection<TopNavItem> _topNavItems;
        //public ObservableCollection<TopNavItem> TopNavItems
        //{
        //    get => _topNavItems;
        //    set
        //    {
        //        _topNavItems = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TopNavItems)));
        //    }
        //}

        //private TopNavItem _selectedNav;
        //public TopNavItem SelectedNav
        //{
        //    get => _selectedNav;
        //    set
        //    {
        //        if (SelectedNav != value)
        //        {
        //            _selectedNav = value;
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNav)));
        //        }
        //    }
        //}
    }
}
