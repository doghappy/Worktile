using System.Collections.Generic;
using Windows.UI.Xaml;
using Worktile.Common;
using Worktile.Main.Models;

namespace Worktile.Main
{
    public class SettingViewModel : BindableBase
    {
        public SettingViewModel()
        {
            LoadLanguages();
            LoadThemes();
        }

        public List<Language> Languages { get; private set; }

        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }

        public List<Theme> Themes { get; private set; }

        private Theme _selectedTheme;
        public Theme SelectedTheme
        {
            get => _selectedTheme;
            set => SetProperty(ref _selectedTheme, value);
        }

        private void LoadLanguages()
        {
            Languages = new List<Language>
            {
                new Language { Text = "简体中文", Value = "zh-Hans-CN" },
                new Language { Text = "English", Value = "en-US" }
            };
        }

        private void LoadThemes()
        {
            string dark = UtilityTool.GetStringFromResources("DarkThemeDisplayName");
            string light = UtilityTool.GetStringFromResources("LightThemeDisplayName");
            string @default = UtilityTool.GetStringFromResources("DefaultThemeDisplayName");
            Themes = new List<Theme>
            {
                new Theme { Text = @default, Value = ElementTheme.Default },
                new Theme { Text = dark, Value = ElementTheme.Dark },
                new Theme { Text = light, Value = ElementTheme.Light }
            };
        }
    }
}
