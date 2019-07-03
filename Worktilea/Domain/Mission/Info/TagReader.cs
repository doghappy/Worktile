using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using System.Collections.Generic;
using Windows.UI.Core;
using System.Threading.Tasks;
using System;
using Worktile.Common;

namespace Worktile.Domain.Mission.Info
{
    class TagReader : CheckBoxReader
    {
        public override string Control => "Tag";

        public override async Task LoadOptionsAsync(PropertyItem item, List<JObject> allProps, CoreDispatcher dispatcher)
        {
            var selectedValues = new List<DropdownItem>();
            foreach (var prop in allProps)
            {
                string id = prop.Value<string>("_id");
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var dropdownItem = new DropdownItem
                    {
                        Text = prop.Value<string>("name"),
                        Value = id,
                        Color = WtColorHelper.GetNewBrush(prop.Value<string>("color"))
                    };
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
