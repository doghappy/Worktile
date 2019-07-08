using System;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Models;
using Worktile.Tool;
using Worktile.Tool.Models;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Message.Details
{
    public sealed partial class PinListPage : Page
    {
        public PinListPage()
        {
            InitializeComponent();
            ViewModel = new PinListViewModel();
        }

        public PinListViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Session = e.Parameter as Session;
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var msg = btn.DataContext as WtMessage.Message;
            await ViewModel.UnPinAsync(msg);
        }

        private async void MessageControl_OnImageMessageClick(WtMessage.Message message)
        {
            var data = new ImageViewerData
            {
                SelectedItem = UtilityTool.GetS3FileUrl(message.Body.Attachment.Id),
                ItemSource = ViewModel.Messages
                    .Where(m => m.Type == WtMessage.MessageType.Image)
                    .Select(m => UtilityTool.GetS3FileUrl(m.Body.Attachment.Id))
                    .ToList()
            };

            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(ImageViewerPage), data);
                Window.Current.Content = frame;
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }
    }
}
