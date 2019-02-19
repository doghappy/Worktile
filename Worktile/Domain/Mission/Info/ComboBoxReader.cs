using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.ApiModel.ApiMissionVnextTask;
using Worktile.Common;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using System.Threading.Tasks;
using System;

namespace Worktile.Domain.Mission.Info
{
    class ComboBoxReader : IPropertyReader
    {
        public string Control => "ComboBox";

        public void Read(Property property, PropertyItem item, Data data)
        {
            JObject task = JObject.FromObject(data.Value);
            string value = JTokenHelper.GetPropertyValue<string>(task, property.Key);
            item.PropertyId = JTokenHelper.GetPropertyValue<string>(task, property.PropertyKey + ".property_id");
            if (value != null)
            {
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
                }
            }
        }

        public async Task LoadOptionsAsync(PropertyItem item, List<JObject> allProps, CoreDispatcher dispatcher)
        {
            foreach (var prop in allProps)
            {
                string text = null;
                if (prop.ContainsKey("name"))
                    text = prop.Value<string>("name");
                else if (prop.ContainsKey("text"))
                    text = prop.Value<string>("text");
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    item.DataSource.Add(new DropdownItem
                    {
                        Text = text,
                        Value = prop.Value<string>("_id")
                    });
                });
            }
            if (item.SelectedValue != null)
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => item.SelectedValue = item.DataSource.Single(p => p.Value == item.SelectedValue.Value));
            }
        }
    }
}
