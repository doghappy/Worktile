using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class DefaultReader : IPropertyReader
    {
        public DefaultReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.Iteration,
                WtTaskPropertyType.DropDown,
                WtTaskPropertyType.Number
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var jObj = JObject.FromObject(item);
            string value = TaskHelper.GetPropertyValue<string>(jObj, column.Key);
            if (value != null)
            {
                if (column.Lookup == null)
                {
                    cell.Text = value;
                }
                else
                {
                    JObject lookup = JObject.FromObject(data.Data.References.Lookups);
                    var jItem = lookup[column.Lookup].Single(l => l["_id"].Value<string>() == value);
                    if (jItem is JObject obj)
                    {
                        if (obj.ContainsKey("name"))
                        {
                            cell.Text = jItem["name"].Value<string>();
                        }
                        else if (obj.ContainsKey("text"))
                        {
                            cell.Text = jItem["text"].Value<string>();
                        }
                    }
                }
            }
        }
    }
}
