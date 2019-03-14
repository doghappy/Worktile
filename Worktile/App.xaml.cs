using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Domain.SocketMessageConverter;
using System.Linq;
using Worktile.Views;
using Worktile.Enums;
using Worktile.Repository;
using Microsoft.EntityFrameworkCore;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Worktile.Common.Communication;
using Worktile.Operators;

namespace Worktile
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            UnhandledException += App_UnhandledException;

            using (var db = new WorktileDbContext())
            {
                db.Database.Migrate();
            }
        }

        private async void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            if (e.Exception is HttpRequestException ex)
            {
                if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                {
                    await ShowNotificationAsync("请确认网络是否连通", NotificationLevel.Danger);
                    return;
                }
            }
            await ShowNotificationAsync(e.Message, NotificationLevel.Danger);
        }

        private async Task OnLaunchedOrActivatedAsync(IActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e is LaunchActivatedEventArgs)
            {
                var args = e as LaunchActivatedEventArgs;

                if (args.PrelaunchActivated == false)
                {
                    if (rootFrame.Content == null)
                    {
                        // 当导航堆栈尚未还原时，导航到第一页，
                        // 并通过将所需信息作为导航参数传入来配置
                        // 参数
                        //rootFrame.Navigate(typeof(Views.SignIn.PasswordSignInPage), args.Arguments);
                        rootFrame.Navigate(typeof(MainPage), args.Arguments);
                    }
                }
            }
            else if (e is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = e as ToastNotificationActivatedEventArgs;
                QueryString args = QueryString.Parse(toastActivationArgs.Argument);
                if (args.Any())
                {
                    string msg = toastActivationArgs.UserInput["msg"].ToString().Trim();
                    if (msg != string.Empty && args["action"] == "reply")
                    {
                        var data = new
                        {
                            fromType = 1,
                            from = args["from"],
                            to = args["to"],
                            toType = int.Parse(args["toType"]),
                            messageType = 1,
                            client = 1,
                            markdown = 1,
                            content = toastActivationArgs.UserInput["msg"].ToString()
                        };
                        await WtSocket.SendMessageAsync(SocketMessageType.Message, data);
                    }
                }
                // If we're loading the app for the first time, place the main page on
                // the back stack so that user can go back after they've been
                // navigated to the specific page
                //if (rootFrame.BackStack.Count == 0)
                //    rootFrame.BackStack.Add(new PageStackEntry(typeof(MainPage), null, null));
            }

            // 确保当前窗口处于活动状态
            Window.Current.Activate();
            CustomTitleBar();
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            await OnLaunchedOrActivatedAsync(e);
        }

        protected override async void OnActivated(IActivatedEventArgs e)
        {
            await OnLaunchedOrActivatedAsync(e);
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }

        private void CustomTitleBar()
        {
            //draw into the title bar
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            //remove the solid-colored backgrounds behind the caption controls and system back button
            var viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.ButtonBackgroundColor = Colors.Transparent;
            viewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            //viewTitleBar.ButtonForegroundColor = (Color)Resources["SystemBaseHighColor"];
            viewTitleBar.ButtonForegroundColor = (Resources["ApplicationForegroundThemeBrush"] as SolidColorBrush).Color;
        }

        public async Task ShowNotificationAsync(string text, NotificationLevel level, int duration = 0)
        {
            if (Window.Current != null && Window.Current.Content is Frame rootFrame)
            {
                if (rootFrame.Content is MainPage mainPage)
                {
                    MainOperator.ShowNotification(text, level, duration);
                    return;
                }
            }
            var dialog = new ContentDialog
            {
                PrimaryButtonText = "关闭",
                DefaultButton = ContentDialogButton.Primary,
                Title = "异常",
                Content = text
            };
            await dialog.ShowAsync();
        }

        #region UreadBadge
        private static int _unreadBadge;
        public static int UnreadBadge
        {
            get => _unreadBadge;
            set
            {
                if (_unreadBadge != value || value == 0)
                {
                    _unreadBadge = value;
                    if (value > 0)
                        UpdateBadgeNumber(value);
                    else
                        BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
                }
            }
        }

        public static void UpdateBadgeNumber(int number)
        {
            XmlDocument badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
            XmlElement badgeElement = badgeXml.SelectSingleNode("/badge") as XmlElement;
            badgeElement.SetAttribute("value", number.ToString());
            BadgeNotification badge = new BadgeNotification(badgeXml);
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            badgeUpdater.Update(badge);
        }
        #endregion
    }
}
