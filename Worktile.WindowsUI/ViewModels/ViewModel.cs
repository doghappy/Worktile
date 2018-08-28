using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.WindowsUI.Common;
using Worktile.WindowsUI.Models.Results;
using Worktile.WindowsUI.Views.Start;

namespace Worktile.WindowsUI.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            //IsAuthorized = Configuration.IsAuthorized;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        static HttpClient httpClient;
        protected static HttpClient HttpClient
        {
            get
            {
                if (httpClient == null)
                {
                    httpClient = new HttpClient();
                }
                return httpClient;
            }
            set
            {
                httpClient = value;
            }
        }

        protected string ApplicationJson => "application/json";

        //private bool isAuthorized;
        //public bool IsAuthorized
        //{
        //    get => isAuthorized;
        //    set
        //    {
        //        isAuthorized = value;
        //        OnPropertyChanged(nameof(IsAuthorized));
        //    }
        //}

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void GoBack()
        {
            var fe = Window.Current.Content as FrameworkElement;
            var frame = fe.GetChild<Frame>("ContentFrame");
            frame.GoBack();
        }

        protected void ShowNotification(string text, int duration = 0)
        {
            var fe = Window.Current.Content as FrameworkElement;
            fe.GetChildren<InAppNotification>().FirstOrDefault()
                .Show(text, duration);
        }

        protected async Task HandleErrorStatusCodeAsync(HttpResponseMessage resMsg)
        {
            switch (resMsg.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    await OnUnauthorizedAsync(resMsg);
                    break;
                default:
                    await OnDefaultAsync(resMsg);
                    break;
            }
        }

        //protected virtual InAppNotification GetNotification()
        //{
        //    var fe = Window.Current.Content as FrameworkElement;
        //    return fe.GetChild<InAppNotification>("MainNotification");
        //}

        protected async virtual Task<T> ReadHttpResponseMessageAsync<T>(HttpResponseMessage resMsg)
        {
            string json = await resMsg.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected async virtual Task OnDefaultAsync(HttpResponseMessage resMsg)
        {
            //var notification = GetNotification();
            var result = await ReadHttpResponseMessageAsync<BaseResult>(resMsg);
            //notification.Show("error: code is " + result.Code, 4000);
            ShowNotification("error: code is " + result.Code, 4000);
        }

        protected async virtual Task OnUnauthorizedAsync(HttpResponseMessage resMsg)
        {
            await OnDefaultAsync(resMsg);
            var fe = Window.Current.Content as FrameworkElement;
            fe.GetChild<Frame>("ContentFrame").Navigate(typeof(EnterpriseSignInPage));
        }
    }
}
