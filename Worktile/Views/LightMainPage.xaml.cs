using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Common.Communication;
using Worktile.Common.Extensions;
using Worktile.Common.Theme;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Services;
using Worktile.Views.Profile;
using Worktile.Views.SignIn;

namespace Worktile.Views
{
    public sealed partial class LightMainPage : Page, INotifyPropertyChanged
    {
        public LightMainPage()
        {
            InitializeComponent();
            Apps = new ObservableCollection<WtApp>();
            _teamService = new TeamService();
            _userService = new UserService();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        readonly TeamService _teamService;
        readonly UserService _userService;
        private static InAppNotification _inAppNotification;
        public CoreWindowActivationState WindowActivationState { get; private set; }
        public ObservableCollection<WtApp> Apps { get; }

        private WtApp _selectedApp;
        public WtApp SelectedApp
        {
            get => _selectedApp;
            set
            {
                if (_selectedApp != value)
                {
                    _selectedApp = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedApp)));
                }
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        private string _logo;
        public string Logo
        {
            get => _logo;
            set
            {
                if (_logo != value)
                {
                    _logo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Logo)));
                }
            }
        }

        private string _wtBackgroundImage;
        public string WtBackgroundImage
        {
            get => _wtBackgroundImage;
            set
            {
                if (_wtBackgroundImage != value)
                {
                    _wtBackgroundImage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WtBackgroundImage)));
                }
            }
        }

        private TethysAvatar _myAvatar;
        public TethysAvatar MyAvatar
        {
            get => _myAvatar;
            set
            {
                if (_myAvatar != value)
                {
                    _myAvatar = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyAvatar)));
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            NetworkHelper.Instance.NetworkChanged += NetworkChanged;
            string domain = ApplicationData.Current.LocalSettings.Values["Domain"]?.ToString();
            if (string.IsNullOrEmpty(domain))
            {
                Frame.Navigate(typeof(PasswordSignInPage));
            }
            else
            {
                WtHttpClient.Domain = domain;
                await LoadPreferencesAsync();
                await LoadTeamInfoAsync();
                await WtSocket.ConnectSocketAsync();
            }
            _inAppNotification = InAppNotification;
            Window.Current.Activated += Window_Activated;
            IsActive = false;
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs e)
        {
            WindowActivationState = e.WindowActivationState;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _inAppNotification = null;
            WtSocket.Dispose();
            Window.Current.Activated -= Window_Activated;
        }

        private async void NetworkChanged(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    var helper = sender as NetworkHelper;
                    if (helper.ConnectionInformation.IsInternetAvailable)
                    {
                        ShowNotification("网络已恢复，正在重新连接……", NotificationLevel.Warning, 4000);
                    }
                    else
                    {
                        ShowNotification("网络已断开，正在重新连接……", NotificationLevel.Danger, 4000);
                        WtSocket.ReConnect();
                    }
                });
            });
        }

        private async Task LoadPreferencesAsync()
        {
            var me = await _userService.GetMeAsync();
            DataSource.ApiUserMeData = me;
            MyAvatar = new TethysAvatar
            {
                DisplayName = DataSource.ApiUserMeData.Me.DisplayName,
                Background = AvatarHelper.GetColorBrush(DataSource.ApiUserMeData.Me.DisplayName),
                Source = AvatarHelper.GetAvatarBitmap(DataSource.ApiUserMeData.Me.Avatar, AvatarSize.X320, FromType.User)
            };

            var img = me.Me.Preferences.BackgroundImage;
            if (img.StartsWith("desktop-") && img.EndsWith(".jpg"))
            {
                WtBackgroundImage = "ms-appx:///Assets/Images/Background/" + img;
            }
            else
            {
                WtBackgroundImage = me.Config.Box.BaseUrl + "background-image/" + img + "/from-s3";
            }

            WtThemeSelector.ChangeTheme(me.Me.Preferences.Theme);

            //switch (me.Me.Preferences.Theme)
            //{
            //    default:
            //        //NavBackground = Application.Current.Resources["PrimaryBrush"] as SolidColorBrush;
            //        {
            //            // 需要更换的Keys https://stackoverflow.com/a/33854662/7771913
            //            Application.Current.Resources["SystemAccentColor"] = Color.FromArgb(255, 34, 215, 187);
            //        }
            //        break;
            //}
        }

        private async Task LoadTeamInfoAsync()
        {
            var team = await _teamService.GetTeamAsync();
            team.Members.ForEach(m => m.ForShowAvatar(AvatarSize.X80));
            DataSource.Team = team;
            Apps.Add(new WtApp
            {
                Name = "message",
                Icon = WtIconHelper.GetAppIcon("message"),
                Icon2 = WtIconHelper.GetAppIcon2("message"),
                DisplayName = "消息"
            });
            for (int i = 0; i < DataSource.Team.Apps.Count; i++)
            {
                Apps.Add(new WtApp
                {
                    Name = DataSource.Team.Apps[i].Name,
                    Icon = WtIconHelper.GetAppIcon(DataSource.Team.Apps[i].Name),
                    Icon2 = WtIconHelper.GetAppIcon2(DataSource.Team.Apps[i].Name),
                    DisplayName = DataSource.Team.Apps[i].DisplayName
                });
                if (i == 2)
                {
                    if (DataSource.Team.Apps.Count > 3)
                    {
                        Apps.Add(new WtApp
                        {
                            Name = "app",
                            Icon = "\ue61b",
                            Icon2 = "\ue61d",
                            DisplayName = "应用"
                        });
                    }
                    break;
                }
            }
            Apps.Add(new WtApp
            {
                Name = "contact",
                Icon = "\ue617",
                Icon2 = "\ue604",
                DisplayName = "通讯录"
            });
            Logo = DataSource.ApiUserMeData.Config.Box.LogoUrl + DataSource.Team.Logo;
            SelectedApp = Apps.First();
        }

        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = sender as ListView;
            if (e.AddedItems.Count > 0)
            {
                var container = lv.ContainerFromItem(e.AddedItems[0]) as FrameworkElement;
                while (container == null)
                {
                    await Task.Delay(20);
                    container = lv.ContainerFromItem(e.AddedItems[0]) as FrameworkElement;
                }
                var uc = container.GetChild<UserControl>("AppItem");
                VisualStateManager.GoToState(uc, "Pressed", false);

                if (_people != null)
                {
                    _people.Background = new SolidColorBrush(Colors.Transparent);
                }
                ContentFrameNavigate(SelectedApp.Name);
            }
            if (e.RemovedItems.Count > 0)
            {
                var container = lv.ContainerFromItem(e.RemovedItems[0]) as FrameworkElement;
                var btn = container.GetChild<UserControl>("AppItem");
                VisualStateManager.GoToState(btn, "Normal", false);
            }
        }

        private void ContentFrameNavigate(string app)
        {
            switch (app)
            {
                case "message":
                    MainContentFrame.Navigate(typeof(Message.MasterPage));
                    break;
                case "contact":
                    MainContentFrame.Navigate(typeof(Contact.MasterPage));
                    break;
                case "app":
                    MainContentFrame.Navigate(typeof(TestPage));
                    break;
                default:
                    MainContentFrame.Navigate(typeof(WaitForDevelopmentPage));
                    break;
            }
        }

        private void ListViewItem_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ListViewItem;
            var icon = item.GetChild<FontIcon>("AppIcon");
            if (SelectedApp != null && icon.Glyph != SelectedApp.Icon)
            {
                var uc = item.GetChild<UserControl>("AppItem");
                VisualStateManager.GoToState(uc, "PointOver", false);
            }
        }

        private void ListViewItem_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != _people)
            {
                var icon = item.GetChild<FontIcon>("AppIcon");
                if (SelectedApp != null && icon.Glyph != SelectedApp.Icon)
                {
                    var uc = item.GetChild<UserControl>("AppItem");
                    VisualStateManager.GoToState(uc, "Normal", false);
                }
            }
        }

        ListViewItem _people;

        private void People_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Todo 如果ContentFrame内容页不是，则导航过去
            SelectedApp = null;
            var item = sender as ListViewItem;
            item.Background = Application.Current.Resources["SystemAccentColorDark3"] as SolidColorBrush;
            _people = item;
            var uc = item.GetChild<UserControl>("AppItem");
            VisualStateManager.GoToState(uc, "Pressed", false);
            MainContentFrame.Navigate(typeof(TestPage));
        }

        public static void ShowNotification(string text, NotificationLevel level, int duration = 0)
        {
            if (_inAppNotification != null)
            {
                if (level == NotificationLevel.Default)
                {
                    _inAppNotification.BorderBrush = Application.Current.Resources["SystemControlForegroundBaseLowBrush"] as SolidColorBrush;
                }
                else
                {
                    string key = level.ToString() + "Brush";
                    _inAppNotification.BorderBrush = Application.Current.Resources[key] as SolidColorBrush;
                }
                _inAppNotification.Show(text, duration);
            }
        }

        private void MyAvatar_Click(object sender, RoutedEventArgs e)
        {
            if (!(MainContentFrame.Content is ProfilePage page))
            {
                SelectedApp = null;
                MainContentFrame.Navigate(typeof(ProfilePage));
            }
        }
    }
}
