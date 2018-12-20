using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Worktile.Tethys
{
    public sealed class IconTextBlock : ContentControl
    {
        public IconTextBlock()
        {
            DefaultStyleKey = typeof(IconTextBlock);
        }



        public FontIcon Icon
        {
            get { return (FontIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(FontIcon), typeof(IconTextBlock), new PropertyMetadata(null));



        //public FontFamily IconFontFamily
        //{
        //    get { return (FontFamily)GetValue(IconFontFamilyProperty); }
        //    set { SetValue(IconFontFamilyProperty, value); }
        //}

        //public static readonly DependencyProperty IconFontFamilyProperty =
        //    DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(IconTextBlock), new PropertyMetadata(new FontFamily("Segoe MDL2 Assets")));

        //public string Glyph
        //{
        //    get { return (string)GetValue(GlyphProperty); }
        //    set { SetValue(GlyphProperty, value); }
        //}

        //public static readonly DependencyProperty GlyphProperty =
        //    DependencyProperty.Register("Glyph", typeof(string), typeof(IconTextBlock), new PropertyMetadata(string.Empty));

        //public string Text
        //{
        //    get { return (string)GetValue(TextProperty); }
        //    set { SetValue(TextProperty, value); }
        //}

        //public static readonly DependencyProperty TextProperty =
        //    DependencyProperty.Register("Text", typeof(string), typeof(IconTextBlock), new PropertyMetadata(string.Empty));
    }
}
