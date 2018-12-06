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
    class DateTimeReader : IPropertyReader
    {
        public DateTimeReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.DateTime
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var jObj = JObject.FromObject(item);
            cell.RenderParam = nameof(DefaultReader);
            if (column.Key.Count(dot => dot == '.') > 0)
            {
                var date = TaskHelper.GetPropertyValue<WtDate>(jObj, column.Key);
                if (date.Date.HasValue)
                    cell.Text = WtDateTimeHelper.ToWtKanbanDate(date.Date.Value);
            }
            else
            {
                string timestamp = jObj[column.Key].Value<string>();
                if (timestamp != null)
                    cell.Text = WtDateTimeHelper.GetDateTime(timestamp).ToWtKanbanDate();
            }
        }
    }
}
