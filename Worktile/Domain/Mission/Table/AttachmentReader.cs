using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class AttachmentReader : IPropertyReader
    {
        public AttachmentReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.File
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var jObj = JObject.FromObject(item);
            string[] files = TaskHelper.GetPropertyValue<string[]>(jObj, column.Key);
            cell.Text = files.Length.ToString();
            cell.RenderParam = nameof(DefaultReader);
        }
    }
}
