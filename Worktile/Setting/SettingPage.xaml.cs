using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Windows.ApplicationModel.Resources.Core;
using System.Threading.Tasks;
using System.Globalization;

namespace Worktile.Setting
{
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
            ViewModel = new SettingViewModel();
        }

        public SettingViewModel ViewModel { get; }

        private void Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedLanguage = e.AddedItems[0] as Models.Language;
            ApplicationLanguages.PrimaryLanguageOverride = ViewModel.SelectedLanguage.Value;
            ResourceContext.GetForCurrentView().Reset();
            ResourceContext.GetForViewIndependentUse().Reset();
            UtilityTool.ReloadMainPage();
        }

        private void Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var theme = e.AddedItems[0] as Models.Theme;
            UtilityTool.ReloadPageTheme(theme.Value, UtilityTool.MainPage);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLanguage();
            LoadTheme();
        }

        private void LoadLanguage()
        {
            string language = ApplicationLanguages.PrimaryLanguageOverride;
            if ((language == string.Empty && CultureInfo.CurrentCulture.Name == "zh-CN") || language == "zh-Hans-CN")
            {
                language = "zh-Hans-CN";
            }
            else
            {
                language = "en-US";
            }
            foreach (var item in ViewModel.Languages)
            {
                if (language == item.Value)
                {
                    ViewModel.SelectedLanguage = item;
                    LanguageComboBox.SelectionChanged += Language_SelectionChanged;
                    return;
                }
            }
        }

        private void LoadTheme()
        {
            foreach (var item in ViewModel.Themes)
            {
                if (item.Value == UtilityTool.MainPage.RequestedTheme)
                {
                    ViewModel.SelectedTheme = item;
                    ThemeComboBox.SelectionChanged += Theme_SelectionChanged;
                    return;
                }
            }
        }
    }
}
