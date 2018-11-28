using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Worktile.Views.Mission.Project
{
    public abstract partial class AbstractPage : Page
    {
        public AbstractPage()
        {
            InitializeComponent();
        }

        protected string AddonId { get; private set; }
        protected string ViewId { get; private set; }
        protected string TaskIdentifierPrefix { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dynamic parameters = e.Parameter;
            AddonId = parameters.AddonId;
            ViewId = parameters.ViewId;
            TaskIdentifierPrefix = parameters.TaskIdentifierPrefix;
        }
    }
}
