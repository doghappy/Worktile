using System;
using System.Text.RegularExpressions;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Worktile.Common.Communication;
using Worktile.ViewModels.SignIn;

namespace Worktile.Views.SignIn
{
    public sealed partial class VerifyCodeSignInPage : Page
    {
        public VerifyCodeSignInPage()
        {
            InitializeComponent();
            ViewModel = new VerifyCodeSignInViewModel();
        }

        VerifyCodeSignInViewModel ViewModel { get; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MobileTextBox.Focus(FocusState.Programmatic);
            ViewModel.Frame = Frame;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            WtHttpClient.Client.DefaultRequestHeaders.Add("app-key", "65dddfa99d3d42758fd9c02ea6554e41");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            WtHttpClient.Client.DefaultRequestHeaders.Remove("app-key");
        }

        private void PasswordSignIn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PasswordSignInPage));
        }

        private async void NextStepButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowError = false;
            if (!Regex.IsMatch(ViewModel.Mobile, @"^\d{7,}$"))
            {
                ViewModel.ErrorText = "手机号输入格式不正确";
                ViewModel.ShowError = true;
            }
            else if (string.IsNullOrEmpty(ViewModel.Code))
            {
                ViewModel.ErrorText = "请输入短信验证码";
                ViewModel.ShowError = true;
            }
            else
            {
                await ViewModel.SignInAsync();
            }
        }

        private void CodeTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ViewModel.Code = CodeTextBox.Text;
                NextStepButton_Click(sender, e);
            }
        }

        private async void SendCode_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowError = false;
            if (!Regex.IsMatch(ViewModel.Mobile, @"^\d{7,}$"))
            {
                ViewModel.ErrorText = "手机号输入格式不正确";
                ViewModel.ShowError = true;
            }
            else if (string.IsNullOrEmpty(ViewModel.ImageCode))
            {
                ViewModel.ErrorText = "请输入图片验证码";
                ViewModel.ShowError = true;
            }
            else
            {
                await ViewModel.SendCodeAsync();
                if (ViewModel.ShowError)
                {
                    ImageButton_Click(sender, e);
                    ImageCodeTextBox.Focus(FocusState.Programmatic);
                }
                else
                {
                    CodeTextBox.Focus(FocusState.Programmatic);
                }
            }
        }

        private async void SendVoice_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowError = false;
            if (!Regex.IsMatch(ViewModel.Mobile, @"^\d{7,}$"))
            {
                ViewModel.ErrorText = "手机号输入格式不正确";
                ViewModel.ShowError = true;
            }
            else if (string.IsNullOrEmpty(ViewModel.ImageCode))
            {
                ViewModel.ErrorText = "请输入图片验证码";
                ViewModel.ShowError = true;
            }
            else
            {
                await ViewModel.SendVoiceAsync();
                if (ViewModel.ShowError)
                {
                    ImageButton_Click(sender, e);
                    ImageCodeTextBox.Focus(FocusState.Programmatic);
                }
                else
                {
                    CodeTextBox.Focus(FocusState.Programmatic);
                }
            }
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://worktile.com/captcha/code/generate?" + Guid.NewGuid();
            ImageButton.Content = new Image
            {
                Source = new BitmapImage(new Uri(url)),
                Height = 30
            };
        }
    }
}
