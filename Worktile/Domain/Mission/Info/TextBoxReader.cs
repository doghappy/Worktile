using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.ApiModel.ApiMissionVnextTask;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using System.Collections.Generic;

namespace Worktile.Domain.Mission.Info
{
    class TextBoxReader : IPropertyReader
    {
        public void Read(Property property, PropertyItem item, Data data)
        {
            JObject task = JObject.FromObject(data.Value);
            item.Value = TaskHelper.GetPropertyValue<string>(task, property.Key);
            item.Control = nameof(TextBox);
        }

        public void LoadOptions(PropertyItem item, List<JObject> allProps)
        {
            throw new System.NotImplementedException();
        }
    }
}
