using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class TagReader : IPropertyReader
    {
        public TagReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.Tag
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var jObj = JObject.FromObject(item);
            string[] tagIds = TaskHelper.GetPropertyValue<string[]>(jObj, column.Key);
            if (tagIds.Length > 0)
            {
                cell.Tags = new List<TagCell>();
                foreach (var id in tagIds)
                {
                    var tag = data.Data.References.Lookups.Tags.Single(t => t.Id == id);
                    cell.Tags.Add(new TagCell
                    {
                        Background = WtColorHelper.GetNewColor(tag.Color),
                        Foreground = "#FFFFFF",
                        Text = tag.Name
                    });
                }
            }
        }
    }
}
