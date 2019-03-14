using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Worktile.Views.Message.Dialog
{
    public sealed partial class FileShareDialg : ContentDialog
    {
        public FileShareDialg()
        {
            InitializeComponent();
            var list = new List<SampleItem>
            {
                new SampleItem { FirstName = "郭", LastName = "靖" },
                new SampleItem { FirstName = "郭", LastName = "芙" },
                new SampleItem { FirstName = "黄", LastName = "蓉" },
                new SampleItem { FirstName = "黄", LastName = "药师" }
            };
            Items = list.GroupBy(s => s.FirstName)
                .Select(s => new SampleItemGroup(s)
                {
                    Key = s.Key
                })
                .ToList();
        }

        List<SampleItemGroup> Items { get; }

        private void SemanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.IsSourceZoomedInView == false)
            {
                e.DestinationItem.Item = e.SourceItem.Item;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }

    class SampleItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    class SampleItemGroup : List<object>
    {
        public SampleItemGroup(IEnumerable<object> items) : base(items) { }

        public string Key { get; set; }
    }
}
