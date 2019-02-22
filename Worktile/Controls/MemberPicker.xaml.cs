using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Worktile.Controls
{
    public sealed partial class MemberPicker : UserControl
    {
        public MemberPicker()
        {
            InitializeComponent();
            Data = new ObservableCollection<int>();
        }

        public ObservableCollection<int> Data { get; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Data.Add(1);
        }
    }
}
