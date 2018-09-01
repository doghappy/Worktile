using System.ComponentModel;
using Worktile.WindowsUI.Common;

namespace Worktile.WindowsUI.ViewModels
{
    public abstract class MainAreaViewModel : ViewModel, INotifyPropertyChanged
    {
        public MainAreaViewModel()
        {
            SetBackgroundImage();
        }

        private string backgroundImage;
        public string BackgroundImage
        {
            get => backgroundImage;
            private set
            {
                backgroundImage = value;
                OnPropertyChanged();
            }
        }


        public void SetBackgroundImage()
        {
            var preference = Configuration.TeamConfig.Me.Preference;
            if (preference.BackgroundImage.StartsWith("desktop-"))
            {
                BackgroundImage = "/Assets/Images/Background/" + preference.BackgroundImage;
            }
            else
            {
                var box = Configuration.TeamConfig.Config.Box;
                int start = box.ServiceUrl.IndexOf("//") + 2;
                int end = box.ServiceUrl.IndexOf('.');
                string from = box.ServiceUrl.Substring(start, end - start);
                BackgroundImage = $"{box.BaseUrl}/background-image/{preference.BackgroundImage}/from-{from}";
            }
        }
    }
}
