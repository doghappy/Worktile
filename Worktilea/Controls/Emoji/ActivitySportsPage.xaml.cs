using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Worktile.Controls.Emoji
{
    sealed partial class ActivitySportsPage : Page
    {
        public ActivitySportsPage()
        {
            InitializeComponent();
        }

        Action<string> _selectEmoji;

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var tb = e.ClickedItem as TextBlock;
            _selectEmoji(tb.Text);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _selectEmoji = e.Parameter as Action<string>;
        }
    }
}
