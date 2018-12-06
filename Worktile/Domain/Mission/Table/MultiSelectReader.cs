using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class MultiSelectReader : IPropertyReader
    {
        public MultiSelectReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.MultiSelect
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var jObj = JObject.FromObject(item);
            string[] ids = TaskHelper.GetPropertyValue<string[]>(jObj, column.Key);
            if (ids.Length > 0)
            {
                cell.Tags = new List<TagCell>();
                JObject lookup = JObject.FromObject(data.Data.References.Lookups);
                foreach (var id in ids)
                {
                    var jItem = lookup[column.Lookup].Single(l => l["_id"].Value<string>() == id);
                    if (jItem is JObject obj)
                    {
                        var tag = new TagCell()
                        {
                            Foreground = "#aaaaaa",
                            Background = "#1aaaaaaa",
                            Text = jItem["text"].Value<string>()
                        };
                        cell.Tags.Add(tag);
                    }
                }
            }
        }
    }
}
