using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Worktile.Enums;
using Worktile.Models;

namespace Worktile.Operators
{
    static class MainOperator
    {
        public static InAppNotification InAppNotification { get; set; }
        public static CoreWindowActivationState WindowActivationState { get; set; }
        public static WtApp SelectedApp { get; set; }
        public static Frame ContentFrame { get; set; }
        public static NavigationView NavView { get; set; }

        public static void ShowNotification(string text, NotificationLevel level, int duration = 0)
        {
            if (InAppNotification != null)
            {
                if (level == NotificationLevel.Default)
                {
                    InAppNotification.BorderBrush = Application.Current.Resources["SystemControlForegroundBaseLowBrush"] as SolidColorBrush;
                }
                else
                {
                    string key = level.ToString() + "Brush";
                    InAppNotification.BorderBrush = Application.Current.Resources[key] as SolidColorBrush;
                }
                InAppNotification.Show(text, duration);
            }
        }
    }
}
