using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Worktile
{
public sealed partial class TestPage : Page, INotifyPropertyChanged
{
    public TestPage()
    {
        InitializeComponent();
        SampleDatas = new List<SampleData>
    {
        new SampleData { Name = "Google", IsChecked = true },
        new SampleData {
            Name = "Microsoft",
            IsChecked = true,
            Chidren = new List<SampleData>
            {
                new SampleData { Name = ".Net", IsChecked = true },
                new SampleData { Name = "Office", IsChecked = false },
                new SampleData { Name = "Windows", IsChecked = true }
            }
        },
        new SampleData { Name = "Apple", IsChecked = true }
    };
    }

    List<SampleData> SampleDatas { get; }

    public event PropertyChangedEventHandler PropertyChanged;
}

class SampleData
{
    public string Name { get; set; }
    public bool IsChecked { get; set; }

    public List<SampleData> Chidren { get; set; }
}
}