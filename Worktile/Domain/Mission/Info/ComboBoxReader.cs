using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.ApiModel.ApiMissionVnextTask;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using System.Collections.Generic;
using System.Linq;

namespace Worktile.Domain.Mission.Info
{
    class ComboBoxReader : IPropertyReader
    {
        public void Read(Property property, PropertyItem item, Data data)
        {
            JObject task = JObject.FromObject(data.Value);
            string value = TaskHelper.GetPropertyValue<string>(task, property.Key);
            if (value != null)
            {
                item.Control = nameof(ComboBox);
                var lookup = JObject.FromObject(data.References.Lookups);
                if (property.Lookup != null)
                {
                    var jItem = lookup[property.Lookup].Single(l => l["_id"].Value<string>() == value);
                    var jObj = jItem as JObject;
                    string text = null;
                    if (jObj.ContainsKey("name"))
                        text = jObj.Value<string>("name");
                    else if (jObj.ContainsKey("text"))
                        text = jObj.Value<string>("text");
                    item.SelectedValue = new DropdownItem
                    {
                        Value = jObj.Value<string>("_id"),
                        Text = text
                    };
                    item.PropertyId = TaskHelper.GetPropertyValue<string>(task, property.PropertyKey + ".property_id");
                }
            }
        }

        public void LoadOptions(PropertyItem item, List<JObject> allProps)
        {
            foreach (var prop in allProps)
            {
                item.DataSource.Add(new DropdownItem
                {
                    Text = prop.Value<string>("name"),
                    Value = prop.Value<string>("_id")
                });
            }
            item.SelectedValue = item.DataSource.Single(p => p.Value == item.SelectedValue.Value);
        }
    }
}
