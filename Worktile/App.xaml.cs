﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Main;
using Worktile.Repository;
using Microsoft.QueryStringDotNET;

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
            string exTitle = UtilityTool.GetStringFromResources("Exception");
            string close = UtilityTool.GetStringFromResources("Close");
            var dialog = new ContentDialog
            {
                PrimaryButtonText = close,
                DefaultButton = ContentDialogButton.Primary,
                Title = exTitle,
                Content = e.Exception.Message
            };
            await dialog.ShowAsync();
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            OnLaunchedOrActivated(e);
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            OnLaunchedOrActivated(e);
        }

        private void OnLaunchedOrActivated(IActivatedEventArgs e)
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
                        //await WtSocket.SendMessageAsync(SocketMessageType.Message, new SendMessageRequestBody
                        //{
                        //    FromType = FromType.User,
                        //    From = args["from"],
                        //    ToType = EnumsNET.Enums.Parse<ToType>(args["toType"]),
                        //    MessageType = MessageType.Text,
                        //    Client = Client.Win8,
                        //    IsMarkdown = true,
                        //    Content = toastActivationArgs.UserInput["msg"].ToString()
                        //});
                        //await WtSocketClient.SendMessage()
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
            //CustomTitleBar();
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
    }
}
