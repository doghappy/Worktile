using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Worktile.Tethys
{
    public sealed class Avatar : Control
    {
        public Avatar()
        {
            DefaultStyleKey = typeof(Avatar);
        }

        //readonly SolidColorBrush[] _brushes = new[]
        //{
        //    //"#2cccda", "#2dbcff", "#4e8af9", "#7076fa", "#9473fd", "#ef7ede", "#99d75a", "#66c060", "#39ba5d"
        //    new SolidColorBrush(Color.FromArgb(255, 44, 204, 218)),
        //    new SolidColorBrush(Color.FromArgb(255, 45, 188, 255)),
        //    new SolidColorBrush(Color.FromArgb(255, 78, 138, 249)),
        //    new SolidColorBrush(Color.FromArgb(255, 112, 118, 250)),
        //    new SolidColorBrush(Color.FromArgb(255, 148, 115, 253)),
        //    new SolidColorBrush(Color.FromArgb(255, 239, 126, 222)),
        //    new SolidColorBrush(Color.FromArgb(255, 153, 215, 90)),
        //    new SolidColorBrush(Color.FromArgb(255, 102, 192, 96)),
        //    new SolidColorBrush(Color.FromArgb(255, 57, 186, 93))
        //};

        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set
            {
                string val = value;
                if (Regex.IsMatch(value, @"^[\u4e00-\u9fa5]+$") && value.Length > 2)
                {
                    val = value.Substring(value.Length - 2, 2);
                }
                else if (Regex.IsMatch(value, @"^[a-zA-Z\/]+$") && value.IndexOf(' ') > 0)
                {
                    var arr = value.Split(' ');
                    val = (arr[0].First().ToString() + arr[1].First()).ToUpper();
                }
                else if (value.Length > 2)
                {
                    val = value.Substring(0, 2).ToUpper();
                }
                else
                {
                    val = value.ToUpper();
                }

                FontSize = (Size / 2) * .8;

                SetValue(DisplayNameProperty, val);
                //int code = value.Sum(n => n);
                //Background = _brushes[code % _brushes.Length];
            }
        }
        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register("DisplayName", typeof(string), typeof(Avatar), new PropertyMetadata(null));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(Avatar), new PropertyMetadata(null));



        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set
            {
                SetValue(SizeProperty, value);
                Width = value;
                Height = value;
                CornerRadius = new CornerRadius(value);
            }
        }
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(Avatar), new PropertyMetadata(100));

        public string BadgeIcon
        {
            get { return (string)GetValue(BadgeIconProperty); }
            set
            {
                SetValue(BadgeIconProperty, value);
                if (!string.IsNullOrEmpty(value))
                {
                    FontSize = Size / 1.6;
                    DisplayNameVisibility = Visibility.Collapsed;
                    BadgeIconVisibility = Visibility.Visible;
                }
            }
        }
        public static readonly DependencyProperty BadgeIconProperty =
            DependencyProperty.Register("BadgeIcon", typeof(string), typeof(Avatar), new PropertyMetadata(null));

        public Visibility DisplayNameVisibility
        {
            get { return (Visibility)GetValue(DisplayNameVisibilityProperty); }
            private set { SetValue(DisplayNameVisibilityProperty, value); }
        }
        public static readonly DependencyProperty DisplayNameVisibilityProperty =
            DependencyProperty.Register("DisplayNameVisibility", typeof(Visibility), typeof(Avatar), new PropertyMetadata(Visibility.Visible));

        public Visibility BadgeIconVisibility
        {
            get { return (Visibility)GetValue(BadgeIconVisibilityProperty); }
            private set { SetValue(BadgeIconVisibilityProperty, value); }
        }
        public static readonly DependencyProperty BadgeIconVisibilityProperty =
            DependencyProperty.Register("BadgeIconVisibility", typeof(Visibility), typeof(Avatar), new PropertyMetadata(Visibility.Collapsed));

        public FontFamily BadgeIconFontFamily
        {
            get { return (FontFamily)GetValue(BadgeIconFontFamilyProperty); }
            set { SetValue(BadgeIconFontFamilyProperty, value); }
        }
        public static readonly DependencyProperty BadgeIconFontFamilyProperty =
            DependencyProperty.Register("BadgeIconFontFamily", typeof(FontFamily), typeof(Avatar), new PropertyMetadata(new FontFamily("Segoe MDL2 Assets")));
    }
}
