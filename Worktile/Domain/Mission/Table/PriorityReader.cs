using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class PriorityReader : IPropertyReader
    {
        public PriorityReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.Priority
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var jObj = JObject.FromObject(item);
            string value = TaskHelper.GetPropertyValue<string>(jObj, column.Key);
            if (value != null)
            {
                var priority = data.Data.References.Lookups.Priorities.Single(p => p.Id == value);
                cell.Foreground = WtColorHelper.GetNewBrush(priority.Color);
                cell.Glyph = WtIconHelper.GetGlyph("wtf-level-" + priority.Icon);
                cell.Text = priority.Name;
            }
        }
    }
}
