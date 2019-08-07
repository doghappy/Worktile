using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Worktile.Models;

namespace Worktile.Controls
{
    public sealed partial class SessionAvatar : UserControl, INotifyPropertyChanged
    {
        public SessionAvatar()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //public Session Session
        //{
        //    get { return (Session)GetValue(SessionProperty); }
        //    set { SetValue(SessionProperty, value); }
        //}
        //public static readonly DependencyProperty SessionProperty =
        //    DependencyProperty.Register("Session", typeof(Session), typeof(SessionAvatar), new PropertyMetadata(null, OnSessionChanged));

        //public string Icon
        //{
        //    get { return (string)GetValue(AvatarFontFamilyProperty); }
        //    set { SetValue(AvatarFontFamilyProperty, value); }
        //}
        //public static readonly DependencyProperty AvatarFontFamilyProperty =
        //    DependencyProperty.Register("AvatarFontFamily", typeof(string), typeof(SessionAvatar), new PropertyMetadata(null));

        //public FontFamily IconFontFamily
        //{
        //    get { return (FontFamily)GetValue(IconFontFamilyProperty); }
        //    set { SetValue(IconFontFamilyProperty, value); }
        //}
        //public static readonly DependencyProperty IconFontFamilyProperty =
        //    DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(SessionAvatar), new PropertyMetadata(null));



        //private static void OnSessionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (e.NewValue != e.OldValue && e.NewValue != null)
        //    {
        //        var session = e.NewValue as Session;
        //        if (session.Type == SessionType.Channel)
        //        {
        //            var sa = sender as SessionAvatar;
        //            if (session.Visibility == WtVisibility.Private)
        //            {
        //                sa.Icon = "\uE748";
        //                sa.IconFontFamily = new FontFamily("ms-appx:///Worktile.Tethys/Assets/Fonts/iconfont.ttf#wtf");
        //            }
        //            else if (session.Visibility == WtVisibility.Public)
        //            {
        //                sa.Icon = "\uE64E";
        //                sa.IconFontFamily = new FontFamily("ms-appx:///Worktile,,,/Assets/Fonts/lc-iconfont.ttf#lcfont");
        //            }
        //        }
        //    }
        //}

        private Session _session;
        public Session Session
        {
            get => _session;
            set
            {
                if (_session != value)
                {
                    _session = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Session)));
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Session != null && Session.Type == SessionType.Channel)
            {
                if (Session.Visibility == WtVisibility.Private)
                {
                    ChannelAvatar.Icon = "\uE748";
                    ChannelAvatar.FontFamily = new FontFamily("ms-appx:///Worktile.Tethys/Assets/Fonts/iconfont.ttf#wtf");
                }
                else if (Session.Visibility == WtVisibility.Public)
                {
                    ChannelAvatar.Icon = "\uE64E";
                    ChannelAvatar.FontFamily = new FontFamily("ms-appx:///Worktile,,,/Assets/Fonts/lc-iconfont.ttf#lcfont");
                }
            }
        }
    }
}
