using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Worktile.Common;

namespace Worktile.Models.KanBan
{
    public class KanBanItem
    {
        public string Id { get; set; }
        public Avatar Avatar { get; set; }
        public TaskState State { get; set; }
        public string Title { get; set; }
        public string Identifier { get; set; }
        public DateTime? ToDate { get; set; }

        public SolidColorBrush ToDateForeground
        {
            get
            {
                if (ToDate.HasValue && ToDate.Value <= DateTime.Now)
                {
                    return WtColorHelper.DangerColor;
                }
                else
                {
                    return Application.Current.Resources["SystemControlForegroundBaseMediumBrush"] as SolidColorBrush;
                }
            }
        }

        public SolidColorBrush ToDateBackground
        {
            get
            {
                if (ToDate.HasValue && ToDate.Value <= DateTime.Now)
                {
                    return WtColorHelper.DangerColor1A;
                }
                else
                {
                    return Application.Current.Resources["SystemControlForegroundBaseLowBrush"] as SolidColorBrush;
                }
            }
        }

        public TaskType TaskType { get; set; }
    }
}
