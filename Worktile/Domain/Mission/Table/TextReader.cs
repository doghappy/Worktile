using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class TextReader : IPropertyReader
    {
        public TextReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.Text,
                WtTaskPropertyType.MultiText
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var jObj = JObject.FromObject(item);
            string value = TaskHelper.GetPropertyValue<string>(jObj, column.Key);
            cell.Text = value;
            cell.RenderParam = nameof(DefaultReader);
        }
    }
}
