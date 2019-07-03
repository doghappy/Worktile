using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.UI.Xaml;
using Worktile.Common;

namespace Worktile.Setting
{
    public class SettingViewModel : BindableBase
    {
        public SettingViewModel()
        {
            LoadLanguages();
            LoadThemes();
        }

        public List<Models.Language> Languages { get; private set; }

        private Models.Language _selectedLanguage;
        public Models.Language SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }

        public List<Models.Theme> Themes { get; private set; }

        private Models.Theme _selectedTheme;
        public Models.Theme SelectedTheme
        {
            get => _selectedTheme;
            set => SetProperty(ref _selectedTheme, value);
        }

        private void LoadLanguages()
        {
            Languages = new List<Models.Language>
            {
                new Models.Language { Text = "简体中文", Value = "zh-Hans-CN" },
                new Models.Language { Text = "English", Value = "en-US" }
            };
        }

        private void LoadThemes()
        {
            string dark = UtilityTool.GetStringFromResources("DarkThemeDisplayName");
            string light = UtilityTool.GetStringFromResources("LightThemeDisplayName");
            string @default = UtilityTool.GetStringFromResources("DefaultThemeDisplayName");
            Themes = new List<Models.Theme>
            {
                new Models.Theme { Text = @default, Value = ElementTheme.Default },
                new Models.Theme { Text = dark, Value = ElementTheme.Dark },
                new Models.Theme { Text = light, Value = ElementTheme.Light }
            };
        }
    }
}
