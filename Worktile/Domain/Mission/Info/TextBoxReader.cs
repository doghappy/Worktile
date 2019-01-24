using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.ApiModel.ApiMissionVnextTask;
using System.Collections.Generic;
using Windows.UI.Core;
using System.Threading.Tasks;
using Worktile.Infrastructure;

namespace Worktile.Domain.Mission.Info
{
    class TextBoxReader : IPropertyReader
    {
        public string Control => "TextBox";

        public void Read(Property property, PropertyItem item, Data data)
        {
            JObject task = JObject.FromObject(data.Value);
            item.Value = JTokenHelper.GetPropertyValue<string>(task, property.Key);
        }

        public Task LoadOptionsAsync(PropertyItem item, List<JObject> allProps, CoreDispatcher dispatcher)
        {
            throw new System.NotImplementedException();
        }
    }
}
