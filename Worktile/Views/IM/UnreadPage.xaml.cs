using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Models.IM.Message;


namespace Worktile.Views.IM
{
    public sealed partial class UnreadPage : Page
    {
        public UnreadPage()
        {
            InitializeComponent();
            Messages = new IncrementalCollection<Message>(LoadMessagesAsync);
        }

        public IncrementalCollection<Message> Messages { get; }

        private async Task<IEnumerable<Message>> LoadMessagesAsync()
        {
            await Task.Delay(1000);
            return new List<Message>
            {
                new Message()
            };
        }
    }
}
