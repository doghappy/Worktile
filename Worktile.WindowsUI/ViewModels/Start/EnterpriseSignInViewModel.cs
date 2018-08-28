using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.WindowsUI.Common;
using Worktile.WindowsUI.Models.Results;
using Worktile.WindowsUI.Views.Start;

namespace Worktile.WindowsUI.ViewModels.Start
{
    public class EnterpriseSignInViewModel : ViewModel, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;

        private string domain;
        public string Domain
        {
            get => domain;
            set
            {
                if (value != domain)
                {
                    domain = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Initialize()
        {
            var vault = new PasswordVault();
            if (vault.RetrieveAll().Any(v => v.Resource == Configuration.DomainVaultResource))
            {
                var crendential = vault.FindAllByResource(Configuration.DomainVaultResource).FirstOrDefault();
                Domain = crendential.UserName;
            }
        }

        public async Task VerifyDomainAsync(bool isAuto)
        {
            if (string.IsNullOrWhiteSpace(Domain))
            {
                ShowNotification("请输入“企业域名”", 5000);
            }
            else
            {
                IsActive = true;
                string url = $"{Configuration.BaseAddress}/api/team/domain/check?domain={Domain}&absolute=true";
                var resMsg = await HttpClient.GetAsync(url);
                if (resMsg.IsSuccessStatusCode)
                {
                    string json = await resMsg.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DataResult<bool>>(json);
                    if (data.Data)
                    {
                        Configuration.BaseAddress = $"https://{Domain}.worktile.com";
                        if (!isAuto)
                        {
                            var frame = Window.Current.Content as Frame;
                            frame.Navigate(typeof(UserSignInPage));
                        }
                    }
                    else
                    {
                        if (isAuto)
                        {
                            var frame = Window.Current.Content as Frame;
                            frame.Navigate(typeof(EnterpriseSignInPage));
                        }
                        else
                        {
                            ShowNotification("你输入的企业域名不存在。", 5000);
                        }
                    }
                }
                else
                {
                    if (isAuto)
                    {
                        var frame = Window.Current.Content as Frame;
                        frame.Navigate(typeof(EnterpriseSignInPage));
                    }
                    else
                    {
                        await HandleErrorStatusCodeAsync(resMsg);
                    }
                }
                IsActive = false;
            }
        }
    }
}
