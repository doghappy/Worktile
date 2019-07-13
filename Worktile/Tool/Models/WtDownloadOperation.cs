using System.ComponentModel;
using Windows.Networking.BackgroundTransfer;

namespace Worktile.Tool.Models
{
    public class WtDownloadOperation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }

        public string Extension { get; set; }

        private double _bytesReceived;
        public double BytesReceived
        {
            get => _bytesReceived;
            set
            {
                if (_bytesReceived != value)
                {
                    _bytesReceived = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BytesReceived)));
                }
            }
        }

        private double _totalBytesToReceive;
        public double TotalBytesToReceive
        {
            get => _totalBytesToReceive;
            set
            {
                if (_totalBytesToReceive != value)
                {
                    _totalBytesToReceive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalBytesToReceive)));
                }
            }
        }

        private double _progressRate;
        public double ProgressRate
        {
            get => _progressRate;
            set
            {
                if (_progressRate != value)
                {
                    _progressRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProgressRate)));
                }
            }
        }

        public DownloadOperation DownloadOperation { get; set; }
    }
}
