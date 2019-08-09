using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Windows.ApplicationModel.Resources.Core;
using System.Globalization;

namespace Worktile.Main
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
            var mainPage = SharedData.GetMainPage();
            UtilityTool.ReloadPageTheme(theme.Value, mainPage);
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
                var mainPage = SharedData.GetMainPage();
                if (item.Value == mainPage.RequestedTheme)
                {
                    ViewModel.SelectedTheme = item;
                    ThemeComboBox.SelectionChanged += Theme_SelectionChanged;
                    return;
                }
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TestPage));
        }
    }
}
