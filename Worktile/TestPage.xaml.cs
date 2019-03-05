using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            InitializeComponent();
            Persons = new ObservableCollection<IPerson>
            {
                new APerson { Name = "Actor" },
                new BPerson { Name = "Bctor" }
            };
        }

        ObservableCollection<IPerson> Persons { get; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Persons)
            {
                item.Name = "foreach";
            }
        }
    }

    public interface IPerson
    {
        string Name { get; set; }
    }

    public class APerson : IPerson, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }
    }

    public class BPerson : IPerson, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }
    }

    public class PersonDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate APersonTemplate { get; set; }
        public DataTemplate BPersonTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item.GetType() == typeof(APerson))
            {
                return APersonTemplate;
            }
            else
            {
                return BPersonTemplate;
            }
        }
    }
}