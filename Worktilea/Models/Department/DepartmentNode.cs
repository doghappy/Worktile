using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using Worktile.Views.Message;

namespace Worktile.Models.Department
{
    public class DepartmentNode : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public DepartmentNodeType Type { get; set; }

        [JsonProperty("addition")]
        public DepartmentNodeAddition Addition { get; set; }

        [JsonProperty("parent")]
        public string Parent { get; set; }

        [JsonProperty("children")]
        public List<DepartmentNode> Children { get; set; }

        public TethysAvatar Avatar { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

    }
}
