using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Worktile.Tethys.Themes
{
    public static class ThemeSelector
    {
        //public static void Change(Theme theme)
        //{
        //    var appRes = Application.Current.Resources.ToList();
        //}

        private static Theme _currentTheme;
        public static Theme CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme != value)
                {
                    var dic = Application.Current.Resources.MergedDictionaries;
                    string oldSource = $"ms-appx:///Worktile.Tethys/Themes/{_currentTheme}.xaml";
                    var oldItem = dic.FirstOrDefault(r => r.Source.ToString() == oldSource);
                    if (oldItem != null)
                    {
                        dic.Remove(oldItem);
                    }
                    dic.Add(GetResourceDictionary(value));
                    _currentTheme = value;
                }
            }
        }

        private static ResourceDictionary GetResourceDictionary(Theme theme)
        {
            return new ResourceDictionary
            {
                Source = new Uri($"ms-appx:///Worktile.Tethys/Themes/{theme.ToString()}.xaml", UriKind.Absolute)
            };
        }
    }
}
