using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;

namespace Worktile.Controls
{
    public sealed partial class WtColorPicker : UserControl
    {
        public WtColorPicker()
        {
            InitializeComponent();
            Colors = WtColorHelper.Map.Select(m => m.NewColor).ToList();
        }

        public List<string> Colors { get; }

        public string SelectedColor
        {
            get { return (string)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(string), typeof(WtColorPicker), new PropertyMetadata(null));
    }
}
