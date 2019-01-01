using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.ApiModel.ApiMissionVnextTask;
using Worktile.Common;
using System.Collections.Generic;
using Windows.UI.Core;
using System;
using System.Threading.Tasks;

namespace Worktile.Domain.Mission.Info
{
    class CheckBoxReader : IPropertyReader
    {
        public virtual string Control => "CheckBox";

        public void Read(Property property, PropertyItem item, Data data)
        {
            JObject task = JObject.FromObject(data.Value);
            string[] values = TaskHelper.GetPropertyValue<string[]>(task, property.Key);
            item.SelectedIds.AddRange(values);
            item.PropertyId = TaskHelper.GetPropertyValue<string>(task, property.PropertyKey + ".property_id");
        }

        public async virtual Task LoadOptionsAsync(PropertyItem item, List<JObject> allProps, CoreDispatcher dispatcher)
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
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    item.DataSource.Add(dropdownItem);
                    if (item.SelectedIds.Contains(id))
                    {
                        selectedValues.Add(dropdownItem);
                    }
                });
            }
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => item.SelectedValues = selectedValues);
        }
    }
}
