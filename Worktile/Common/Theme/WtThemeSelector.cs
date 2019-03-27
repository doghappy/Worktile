using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Views;
using EnumsNET;

namespace Worktile.Common.Theme
{
    public static class WtThemeSelector
    {
        static WtThemeSelector()
        {
            _themeDic = new Dictionary<WtTheme, WtAccentColor>
            {
                {
                    WtTheme.Default,
                    new WtAccentColor
                    {
                        Accent = Color.FromArgb(255, 34, 215, 187),
                        SystemAccentColorLight1 = Color.FromArgb(255, 107, 232, 213),
                        SystemAccentColorLight2 = Color.FromArgb(255, 82, 226, 205),
                        SystemAccentColorLight3 = Color.FromArgb(255, 58, 221, 196),
                        SystemAccentColorDark1 = Color.FromArgb(255, 27, 193, 166),
                        SystemAccentColorDark2 = Color.FromArgb(255, 20, 171, 145),
                        SystemAccentColorDark3 = Color.FromArgb(255, 14, 150, 124)
                    }
                },
                {
                    WtTheme.BrightPurple,
                    new WtAccentColor
                    {
                        Accent = Color.FromArgb(255, 78, 138, 250),
                        SystemAccentColorLight1 = Color.FromArgb(255, 140, 182, 253),
                        SystemAccentColorLight2 = Color.FromArgb(255, 120, 168, 252),
                        SystemAccentColorLight3 = Color.FromArgb(255, 99, 153, 251),
                        SystemAccentColorDark1 = Color.FromArgb(255, 62, 120, 229),
                        SystemAccentColorDark2 = Color.FromArgb(255, 47, 102, 209),
                        SystemAccentColorDark3 = Color.FromArgb(255, 31, 85, 188)
                    }
                },
                {
                    WtTheme.BrightYellow,
                    new WtAccentColor
                    {
                        Accent = Color.FromArgb(255, 254, 188, 32),
                        SystemAccentColorLight1 = Color.FromArgb(255, 255, 213, 109),
                        SystemAccentColorLight2 = Color.FromArgb(255, 254, 205, 83),
                        SystemAccentColorLight3 = Color.FromArgb(255, 254, 196, 58),
                        SystemAccentColorDark1 = Color.FromArgb(255, 229, 167, 26),
                        SystemAccentColorDark2 = Color.FromArgb(255, 204, 145, 19),
                        SystemAccentColorDark3 = Color.FromArgb(255, 179, 124, 13)
                    }
                },
                {
                    WtTheme.BrownBlack,
                    new WtAccentColor
                    {
                        Accent = Color.FromArgb(255, 72, 72, 72),
                        SystemAccentColorLight1 = Color.FromArgb(255, 141, 141, 141),
                        SystemAccentColorLight2 = Color.FromArgb(255, 118, 118, 118),
                        SystemAccentColorLight3 = Color.FromArgb(255, 95, 95, 95),
                        SystemAccentColorDark1 = Color.FromArgb(255, 62, 62, 62),
                        SystemAccentColorDark2 = Color.FromArgb(255, 51, 51, 51),
                        SystemAccentColorDark3 = Color.FromArgb(255, 41, 41, 41)
                    }
                },
                {
                    WtTheme.Olivine,
                    new WtAccentColor
                    {
                        Accent = Color.FromArgb(255, 102, 192, 96),
                        SystemAccentColorLight1 = Color.FromArgb(255, 155, 217, 151),
                        SystemAccentColorLight2 = Color.FromArgb(255, 137, 209, 133),
                        SystemAccentColorLight3 = Color.FromArgb(255, 120, 200, 114),
                        SystemAccentColorDark1 = Color.FromArgb(255, 82, 171, 77),
                        SystemAccentColorDark2 = Color.FromArgb(255, 61, 151, 58),
                        SystemAccentColorDark3 = Color.FromArgb(255, 41, 130, 39)
                    }
                },
                {
                    WtTheme.Purple,
                    new WtAccentColor
                    {
                        Accent = Color.FromArgb(255, 72, 82, 92),
                        SystemAccentColorLight1 = Color.FromArgb(255, 136, 148, 160),
                        SystemAccentColorLight2 = Color.FromArgb(255, 114, 126, 138),
                        SystemAccentColorLight3 = Color.FromArgb(255, 93, 104, 115),
                        SystemAccentColorDark1 = Color.FromArgb(255, 58, 71, 85),
                        SystemAccentColorDark2 = Color.FromArgb(255, 43, 60, 77),
                        SystemAccentColorDark3 = Color.FromArgb(255, 29, 49, 70)
                    }
                },
                {
                    WtTheme.QQBlue,
                    new WtAccentColor
                    {
                        Accent = Color.FromArgb(255, 45, 188, 255),
                        SystemAccentColorLight1 = Color.FromArgb(255, 113, 214, 255),
                        SystemAccentColorLight2 = Color.FromArgb(255, 91, 206, 255),
                        SystemAccentColorLight3 = Color.FromArgb(255, 68, 197, 255),
                        SystemAccentColorDark1 = Color.FromArgb(255, 36, 168, 234),
                        SystemAccentColorDark2 = Color.FromArgb(255, 27, 148, 213),
                        SystemAccentColorDark3 = Color.FromArgb(255, 18, 128, 191)
                    }
                },
                {
                    WtTheme.SkyBlue,
                    new WtAccentColor
                    {
                        Accent = Color.FromArgb(255, 111, 118, 250),
                        SystemAccentColorLight1 = Color.FromArgb(255, 129, 135, 251),
                        SystemAccentColorLight2 = Color.FromArgb(255, 147, 152, 252),
                        SystemAccentColorLight3 = Color.FromArgb(255, 164, 169, 253),
                        SystemAccentColorDark1 = Color.FromArgb(255, 89, 101, 229),
                        SystemAccentColorDark2 = Color.FromArgb(255, 67, 84, 208),
                        SystemAccentColorDark3 = Color.FromArgb(255, 44, 68, 187)
                    }
                }
            };
        }

        private static WtTheme _currentTheme;

        private static readonly Dictionary<WtTheme, WtAccentColor> _themeDic;

        public static void ChangeTheme(WtTheme theme)
        {
            if (theme != _currentTheme)
            {
                _currentTheme = theme;
                var rootFrame = Window.Current.Content as Frame;
                var mainPage = rootFrame.GetChildren<LightMainPage>().First();

                var themeData = _themeDic.Single(t => t.Key == theme);
                ColorPaletteResources light = FindColorPaletteResourcesForTheme("Light");
                light.Accent = themeData.Value.Accent;
                ColorPaletteResources def = FindColorPaletteResourcesForTheme("Default");
                def.Accent = themeData.Value.Accent;

                Application.Current.Resources["SystemAccentColorDark1"] = themeData.Value.SystemAccentColorDark1;
                Application.Current.Resources["SystemAccentColorDark2"] = themeData.Value.SystemAccentColorDark2;
                Application.Current.Resources["SystemAccentColorDark3"] = themeData.Value.SystemAccentColorDark3;
                Application.Current.Resources["SystemAccentColorLight1"] = themeData.Value.SystemAccentColorLight1;
                Application.Current.Resources["SystemAccentColorLight2"] = themeData.Value.SystemAccentColorLight2;
                Application.Current.Resources["SystemAccentColorLight3"] = themeData.Value.SystemAccentColorLight3;

                ReloadPageTheme(mainPage.RequestedTheme, mainPage);
            }
        }

        public static void ChangeTheme(string theme)
        {
            var wtTheme = EnumsNET.Enums.GetMembers<WtTheme>()
                  .Single(e => e.AsString(EnumFormat.Description) == theme)
                  .Value;
            ChangeTheme(wtTheme);
        }

        private static void ReloadPageTheme(ElementTheme startTheme, LightMainPage mainPage)
        {
            if (mainPage.RequestedTheme == ElementTheme.Dark)
                mainPage.RequestedTheme = ElementTheme.Light;
            else if (mainPage.RequestedTheme == ElementTheme.Light)
                mainPage.RequestedTheme = ElementTheme.Default;
            else if (mainPage.RequestedTheme == ElementTheme.Default)
                mainPage.RequestedTheme = ElementTheme.Dark;

            if (mainPage.RequestedTheme != startTheme)
                ReloadPageTheme(startTheme, mainPage);
        }

        private static ColorPaletteResources FindColorPaletteResourcesForTheme(string theme)
        {
            foreach (var themeDictionary in Application.Current.Resources.ThemeDictionaries)
            {
                if (themeDictionary.Key.ToString() == theme)
                {
                    if (themeDictionary.Value is ColorPaletteResources)
                    {
                        return themeDictionary.Value as ColorPaletteResources;
                    }
                    else if (themeDictionary.Value is ResourceDictionary targetDictionary)
                    {
                        foreach (var mergedDictionary in targetDictionary.MergedDictionaries)
                        {
                            if (mergedDictionary is ColorPaletteResources)
                            {
                                return mergedDictionary as ColorPaletteResources;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
