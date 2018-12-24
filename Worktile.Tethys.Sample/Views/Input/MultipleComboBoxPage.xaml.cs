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
using Worktile.Tethys.Sample.Models;

namespace Worktile.Tethys.Sample.Views.Input
{
    public sealed partial class MultipleComboBoxPage : Page
    {
        public MultipleComboBoxPage()
        {
            InitializeComponent();
            Persons = new List<Person>
            {
                new Person { Id = 1, Name = "Bob" },
                new Person { Id = 2, Name = "Mary" },
                new Person { Id = 2, Name = "Mary" },
                new Person { Id = 2, Name = "HeroWong" }
            };
        }

        List<Person> Persons { get; }
    }
}
