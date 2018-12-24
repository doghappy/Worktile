using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.ApiModel.ApiMissionVnextTask;
using Worktile.Common;
using System.Collections.Generic;

namespace Worktile.Domain.Mission.Info
{
    class CheckBoxReader : IPropertyReader
    {
        public void Read(Property property, PropertyItem item, Data data)
        {
            item.Control = "CheckBox";
            JObject task = JObject.FromObject(data.Value);
            string[] values = TaskHelper.GetPropertyValue<string[]>(task, property.Key);
            item.SelectedIds.AddRange(values);
            item.PropertyId = TaskHelper.GetPropertyValue<string>(task, property.PropertyKey + ".property_id");
        }

        public void LoadOptions(PropertyItem item, List<JObject> allProps)
        {
            var selectedValues = new List<DropdownItem>();
            foreach (var prop in allProps)
            {
                string id = prop.Value<string>("_id");
                var dropdownItem = new DropdownItem
                {
                    Text = prop.Value<string>("text"),
                    Value = id
                };
                item.DataSource.Add(dropdownItem);
                if (item.SelectedIds.Contains(id))
                {
                    selectedValues.Add(dropdownItem);
                }
            }
            item.SelectedValues = selectedValues;
        }
    }
}
