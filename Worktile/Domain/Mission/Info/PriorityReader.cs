using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.ApiModel.ApiMissionVnextTask;
using System.Linq;
using System.Collections.Generic;
using Windows.UI.Core;
using System.Threading.Tasks;
using System;
using Worktile.ViewModels.Infrastructure;
using Worktile.Infrastructure;

namespace Worktile.Domain.Mission.Info
{
    class PriorityReader : IPropertyReader
    {
        public string Control => "Priority";

        public void Read(Property property, PropertyItem item, Data data)
        {
            JObject task = JObject.FromObject(data.Value);
            string value = JTokenHelper.GetPropertyValue<string>(task, property.Key);
            if (value != null)
            {
                var lookup = JObject.FromObject(data.References.Lookups);
                if (property.Lookup != null)
                {
                    var jItem = lookup[property.Lookup].Single(l => l["_id"].Value<string>() == value);
                    var jObj = jItem as JObject;
                    item.SelectedValue = new DropdownItem
                    {
                        Glyph = WtIconHelper.GetGlyph("wtf-level-" + jObj.Value<string>("icon")),
                        Text = jObj.Value<string>("name"),
                        Value = jObj.Value<string>("_id"),
                        Color = WtColorHelper.GetNewBrush(jObj.Value<string>("color"))
                    };
                    item.PropertyId = JTokenHelper.GetPropertyValue<string>(task, property.PropertyKey + ".property_id");
                }
            }
        }

        public async Task LoadOptionsAsync(PropertyItem item, List<JObject> allProps, CoreDispatcher dispatcher)
        {
            foreach (var prop in allProps)
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => item.DataSource.Add(new DropdownItem
                    {
                        Color = WtColorHelper.GetNewBrush(prop.Value<string>("color")),
                        Glyph = WtIconHelper.GetGlyph("wtf-level-" + prop.Value<string>("icon")),
                        Text = prop.Value<string>("name"),
                        Value = prop.Value<string>("_id")
                    }));
            }
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => item.SelectedValue = item.DataSource.Single(p => p.Value == item.SelectedValue.Value));
        }
    }
}
