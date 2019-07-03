using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.Team;
using Worktile.NavigateModels.SignIn;
using Worktile.ViewModels.SignIn;

namespace Worktile.Views.SignIn
{
    public sealed partial class ChooseTeamPage : Page, INotifyPropertyChanged
    {
        public ChooseTeamPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ToChooseTeamParam _param;

        ChooseTeamViewModel ViewModel { get; set; }

        private List<Team> _teams;
        public List<Team> Teams
        {
            get => _teams;
            set
            {
                if (_teams != value)
                {
                    _teams = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Teams)));
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _param = e.Parameter as ToChooseTeamParam;
            ViewModel = new ChooseTeamViewModel(Frame);
            Teams = _param.Teams;
            foreach (var item in Teams)
            {
                item.Logo = "https://s3.cn-north-1.amazonaws.com.cn/lclogo/" + item.Logo;
            }
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var lv = sender as ListView;
            lv.IsItemClickEnabled = false;
            var team = e.ClickedItem as Team;
            await ViewModel.SignInAsync(team.Id, team.Domain, _param.PassToken);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PasswordSignInPage));
        }
    }
}
