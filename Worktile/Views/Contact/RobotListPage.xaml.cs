using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Models;

namespace Worktile.Views.Contact
{
    public sealed partial class RobotListPage : Page
    {
        public RobotListPage()
        {
            InitializeComponent();
            Avatars = new ObservableCollection<TethysAvatar>();
        }

        ObservableCollection<TethysAvatar> Avatars { get; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var avatars = DataSource.Team.Members
                 .Where(m => m.Role == Enums.RoleType.Bot)
                 .Select(m => m.TethysAvatar);
            foreach (var item in avatars)
            {
                Avatars.Add(item);
            }
        }
    }
}
