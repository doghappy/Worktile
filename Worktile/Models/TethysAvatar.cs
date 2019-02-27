﻿using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Worktile.Models
{
    public class TethysAvatar
    {
        public string Id { get; set; }
        public BitmapImage Source { get; set; }
        public string DisplayName { get; set; }
        public string Icon { get; set; }
        public SolidColorBrush Background { get; set; }
        public SolidColorBrush Foreground { get; set; }
        public FontFamily AvatarFont { get; set; }
        public IEnumerable<string> DisplayNamePinyin { get; set; }
        public string Name { get; set; }
    }
}