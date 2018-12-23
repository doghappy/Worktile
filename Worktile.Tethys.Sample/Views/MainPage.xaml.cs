using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Tethys.Sample.Models;

namespace Worktile.Tethys.Sample.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            Navs = new List<NavItem>
            {
                new NavItem
                {
                    Name = "Basic",
                    Glyph = "\uE10F",
                    Children = new List<NavItem>
                    {
                        new NavItem
                        {
                            Name = "Color",
                            Glyph = "\ue790",
                            SourcePageType = typeof(Basic.ColorPage)
                        },
                        new NavItem
                        {
                            Name = "Wtf",
                            Glyph = "\ue164",
                            SourcePageType = typeof(Basic.WtfPage)
                        }
                    }
                },
                new NavItem
                {
                    Name = "Data Display",
                    Glyph = "\uE7BC",
                    Children = new List<NavItem>
                    {
                        new NavItem
                        {
                            Name = "Label",
                            Glyph = "\ue930",
                            SourcePageType = typeof(DataDisplay.LabelPage)
                        }
                    }
                },
                new NavItem
                {
                    Name = "Input",
                    Glyph = "\uE0A2",
                    Children = new List<NavItem>
                    {
                        new NavItem
                        {
                            Name = "MultipleComboBox",
                            Glyph = "\ue179",
                            SourcePageType = typeof(Input.MultipleComboBoxPage)
                        }
                    }
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        List<NavItem> Navs { get; }

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
                    ContentFrame.Navigate(typeof(GridViewPage), value.Children);
                }
            }
        }


        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedNav = Navs.First();
        }
    }
}
