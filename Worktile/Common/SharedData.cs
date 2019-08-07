using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Worktile.Main;
using Worktile.Models;

namespace Worktile.Common
{
    public static class SharedData
    {
        static SharedData()
        {
            SetAvatarBackgroundBrushes();
        }

        public static SolidColorBrush[] AvatarBackgroundBrushes { get; private set; }

        private static void SetAvatarBackgroundBrushes()
        {
            AvatarBackgroundBrushes = new[]
            {
                //"#2cccda", "#2dbcff", "#4e8af9", "#7076fa", "#9473fd", "#ef7ede", "#99d75a", "#66c060", "#39ba5d"
                new SolidColorBrush(Color.FromArgb(255, 44, 204, 218)),
                new SolidColorBrush(Color.FromArgb(255, 45, 188, 255)),
                new SolidColorBrush(Color.FromArgb(255, 78, 138, 249)),
                new SolidColorBrush(Color.FromArgb(255, 112, 118, 250)),
                new SolidColorBrush(Color.FromArgb(255, 148, 115, 253)),
                new SolidColorBrush(Color.FromArgb(255, 239, 126, 222)),
                new SolidColorBrush(Color.FromArgb(255, 153, 215, 90)),
                new SolidColorBrush(Color.FromArgb(255, 102, 192, 96)),
                new SolidColorBrush(Color.FromArgb(255, 57, 186, 93))
            };
        }

        public static StorageBox Box { get; set; }

        public static LightMainPage GetMainPage()
        {
            var rootFrame = Window.Current.Content as Frame;
            return rootFrame.Content as LightMainPage;
        }
    }
}
