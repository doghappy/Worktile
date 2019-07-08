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

namespace Worktile
{
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            InitializeComponent();
            Images = new ObservableCollection<string>
            {
                "ms-appx:///Assets/Images/Background/desktop-1.jpg",
                "ms-appx:///Assets/Images/Background/desktop-2.jpg",
                "ms-appx:///Assets/Images/Background/desktop-3.jpg",
                "ms-appx:///Assets/Images/Background/desktop-4.jpg",
                "ms-appx:///Assets/Images/Background/desktop-5.jpg",
                "ms-appx:///Assets/Images/Background/desktop-6.jpg",
                "ms-appx:///Assets/Images/Background/desktop-7.jpg",
            };
        }

        public ObservableCollection<string> Images { get; }
    }
}
