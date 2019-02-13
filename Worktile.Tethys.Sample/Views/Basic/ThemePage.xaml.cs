using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Tethys.Themes;

namespace Worktile.Tethys.Sample.Views.Basic
{
    public sealed partial class ThemePage : Page, INotifyPropertyChanged
    {
        public ThemePage()
        {
            InitializeComponent();
            Themes = new List<ThemeItem>
            {
                new ThemeItem { Name = "碧波万顷", Brush = Application.Current.Resources["DefaultThemeBrush"] as SolidColorBrush, Theme = Theme.Default },
                new ThemeItem { Name = "千岩竞秀", Brush = Application.Current.Resources["BrownBlackThemeBrush"] as SolidColorBrush, Theme = Theme.BrownBlack },
                new ThemeItem { Name = "紫气东来", Brush = Application.Current.Resources["SkyBlueThemeBrush"] as SolidColorBrush, Theme = Theme.SkyBlue },
                new ThemeItem { Name = "水墨丹青", Brush = Application.Current.Resources["PurpleThemeBrush"] as SolidColorBrush, Theme = Theme.Purple },
                new ThemeItem { Name = "郁郁葱葱", Brush = Application.Current.Resources["OlivineThemeBrush"] as SolidColorBrush, Theme = Theme.Olivine },
                new ThemeItem { Name = "午后暖阳", Brush = Application.Current.Resources["BrightYellowThemeBrush"] as SolidColorBrush, Theme = Theme.BrightYellow },
                new ThemeItem { Name = "湖光山色", Brush = Application.Current.Resources["QQBlueThemeBrush"] as SolidColorBrush, Theme = Theme.QQBlue },
                new ThemeItem { Name = "青出于蓝", Brush = Application.Current.Resources["BrightPurpleThemeBrush"] as SolidColorBrush, Theme = Theme.BrightPurple }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<ThemeItem> Themes { get; }

        private ThemeItem _selectedTheme;
        public ThemeItem SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;
                    ThemeSelector.CurrentTheme = value.Theme;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTheme)));
                }
            }
        }

    }

    public class ThemeItem
    {
        public string Name { get; set; }
        public Theme Theme { get; set; }
        public SolidColorBrush Brush { get; set; }
    }
}
