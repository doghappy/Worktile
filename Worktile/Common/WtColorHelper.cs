using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Worktile.Models;

namespace Worktile.Common
{
    static class WtColorHelper
    {
        static WtColorHelper()
        {
            Map = new List<ColorMapItem>
            {
                new ColorMapItem { Color = "#a7cfae", NewColor = "#22d7bb" },
                new ColorMapItem { Color = "#9ceceb", NewColor = "#18bfa4" },
                new ColorMapItem { Color = "#a1e1e6", NewColor = "#2cccda" },
                new ColorMapItem { Color = "#92d8d8", NewColor = "#2dbcff" },
                new ColorMapItem { Color = "#90cccc", NewColor = "#4e8af9" },
                new ColorMapItem { Color = "#a5d8f5", NewColor = "#7076fa" },
                new ColorMapItem { Color = "#bed3f6", NewColor = "#9473fd" },
                new ColorMapItem { Color = "#a0c6e5", NewColor = "#c472ee" },
                new ColorMapItem { Color = "#75aad2", NewColor = "#ef7ede" },
                new ColorMapItem { Color = "#fec2c1", NewColor = "#f969aa" },
                new ColorMapItem { Color = "#fedebb", NewColor = "#fc587b" },
                new ColorMapItem { Color = "#f5e9aa", NewColor = "#fa5a55" },
                new ColorMapItem { Color = "#ebb892", NewColor = "#ff7747" },
                new ColorMapItem { Color = "#eda395", NewColor = "#ffa415" },
                new ColorMapItem { Color = "#dfb3a5", NewColor = "#ffd234" },
                new ColorMapItem { Color = "#f5aac8", NewColor = "#99d75a" },
                new ColorMapItem { Color = "#c495af", NewColor = "#66c060" },
                new ColorMapItem { Color = "#cebdf3", NewColor = "#39ba5d" }
            };
        }

        public static List<ColorMapItem> Map { get; }

        public static string GetNewColor(string color)
        {
            var item = Map.SingleOrDefault(c => c.Color == color);
            if (item == null)
            {
                return color;
            }
            return item.NewColor;
        }

        public static SolidColorBrush GetNewBrush(string color)
        {
            color = GetNewColor(color);
            return GetSolidColorBrush(color);
        }

        public static string GetColorByClass(string cls)
        {
            string color = null;
            switch (cls)
            {
                case "task": color = "#22d7bb"; break;
                case "worksheet": color = "#9473fd"; break;
                case "demand": color = "#66c060"; break;
                case "bug": color = "#fa5a55"; break;
                case "pc": color = "#2dbcff"; break;
                case "ios": color = "#4e8af9"; break;
                case "android": color = "#99d75a"; break;
                case "man": color = "#2cccda"; break;
            }
            return color;
        }

        public static SolidColorBrush GetSolidColorBrush(string hex)
        {
            if (hex[0] == '#')
            {
                hex = hex.Replace("#", string.Empty);
            }
            if (hex.Length == 6)
            {
                byte r = (byte)Convert.ToUInt32(hex.Substring(0, 2), 16);
                byte g = (byte)Convert.ToUInt32(hex.Substring(2, 2), 16);
                byte b = (byte)Convert.ToUInt32(hex.Substring(4, 2), 16);
                return new SolidColorBrush(Color.FromArgb(255, r, g, b));
            }
            else if (hex.Length == 8)
            {
                byte a = (byte)Convert.ToUInt32(hex.Substring(0, 2), 16);
                byte r = (byte)Convert.ToUInt32(hex.Substring(2, 2), 16);
                byte g = (byte)Convert.ToUInt32(hex.Substring(4, 2), 16);
                byte b = (byte)Convert.ToUInt32(hex.Substring(6, 2), 16);
                return new SolidColorBrush(Color.FromArgb(26, r, g, b));
            }
            else
            {

                throw new ArgumentException();
            }
        }

        public static SolidColorBrush DangerColor => new SolidColorBrush(Color.FromArgb(255, 255, 91, 87));
        public static SolidColorBrush DangerColor1A => new SolidColorBrush(Color.FromArgb(26, 255, 91, 87));

        public static SolidColorBrush GetForegroundWithExpire(DateTime date)
        {
            return date <= DateTime.Now
                ? DangerColor
                : GetSolidColorBrush("#aaaaaa");
        }

        public static SolidColorBrush GetBackgroundWithExpire(DateTime date)
        {
            return date <= DateTime.Now
                ? DangerColor1A
                : GetSolidColorBrush("#1aaaaaaa");
        }
    }
}
