using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class DateSpanReader : IPropertyReader
    {
        public DateSpanReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.DateSpan
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var jObj = JObject.FromObject(item);
            var span = TaskHelper.GetPropertyValue<DateSpan>(jObj, column.Key);
            if (span.Begin.Date.HasValue && span.End.Date.HasValue)
            {
                cell.Text = WtDateTimeHelper.ToWtKanbanDate(span.Begin.Date.Value)
                    + " ~ "
                    + WtDateTimeHelper.ToWtKanbanDate(span.End.Date.Value);
                cell.RenderParam = nameof(DefaultReader);
            }
        }
    }
}
