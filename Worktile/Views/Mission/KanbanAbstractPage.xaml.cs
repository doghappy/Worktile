using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Worktile.Common;
using Worktile.Models.Kanban;

namespace Worktile.Views.Mission
{
    public abstract partial class KanbanAbstractPage : Page
    {
        public KanbanAbstractPage()
        {
            InitializeComponent();
        }

        protected bool IsPageLoaded { get; set; }
        protected abstract Grid MyGrid { get; }

        protected void ReadForProgressBar(KanbanGroup kbGroup, int state)
        {
            switch (state)
            {
                case 1: kbGroup.NotStarted++; break;
                case 2: kbGroup.Processing++; break;
                case 3: kbGroup.Completed++; break;
            }
        }

        protected KanbanItemProperty GetDueProperty(DateTime? dueDate)
        {
            if (dueDate.HasValue)
            {
                var prop = new KanbanItemProperty
                {
                    Text = "截止时间："
                };
                if (dueDate.Value == new DateTime(dueDate.Value.Year, dueDate.Value.Month, dueDate.Value.Day))
                {
                    prop.Text += dueDate.Value.ToShortDateString();
                }
                else
                {
                    prop.Text += dueDate.Value.ToLocalTime();
                }
                if (dueDate.Value <= DateTime.Now)
                {
                    prop.Foreground = WtColorHelper.DangerColor;
                    prop.Background = WtColorHelper.DangerColor1A;
                }
                else
                {
                    prop.Foreground = Resources["SystemControlForegroundBaseMediumBrush"] as SolidColorBrush;
                    prop.Background = Resources["SystemControlForegroundBaseLowBrush"] as SolidColorBrush;
                }
                return prop;
            }
            return null;
        }

        protected async void MyGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var sp = MyGrid.GetChild<StackPanel>("MissionHeader");
            var lvs = MyGrid.GetChildren<ListView>("DataList");
            if (sp != null && lvs.Any())
            {
                foreach (var item in lvs)
                {
                    item.MaxHeight = MyGrid.ActualHeight - 24 - sp.ActualHeight;
                }
            }
            else
            {
                for (int i = 0; i < 200; i++)
                {
                    if (IsPageLoaded)
                    {
                        sp = MyGrid.GetChild<StackPanel>("MissionHeader");
                        lvs = MyGrid.GetChildren<ListView>("DataList");
                        if (sp != null && lvs.Any())
                        {
                            foreach (var item in lvs)
                            {
                                item.MaxHeight = MyGrid.ActualHeight - 24 - sp.ActualHeight;
                            }
                            return;
                        }
                    }
                    await Task.Delay(100);
                }
            }
        }
    }
}
