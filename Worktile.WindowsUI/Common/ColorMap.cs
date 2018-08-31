using System.Collections.Generic;
using System.Linq;
using Worktile.WindowsUI.Models;

namespace Worktile.WindowsUI.Common
{
    static class ColorMap
    {
        static ColorMap()
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
    }
}
