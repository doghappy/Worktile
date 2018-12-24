using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.ApiModel.ApiMissionVnextTask;
using Worktile.Common;
using System.Linq;
using System.Collections.Generic;

namespace Worktile.Domain.Mission.Info
{
    class PriorityReader : IPropertyReader
    {
        public void Read(Property property, PropertyItem item, Data data)
        {
            JObject task = JObject.FromObject(data.Value);
            string value = TaskHelper.GetPropertyValue<string>(task, property.Key);
            if (value != null)
            {
                item.Control = "Priority";
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
                    Color = WtColorHelper.GetNewBrush(prop.Value<string>("color")),
                    Glyph = WtIconHelper.GetGlyph("wtf-level-" + prop.Value<string>("icon")),
                    Text = prop.Value<string>("name"),
                    Value = prop.Value<string>("_id")
                });
            }
            item.SelectedValue = item.DataSource.Single(p => p.Value == item.SelectedValue.Value);
        }
    }
}
