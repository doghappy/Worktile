using System.Collections.Generic;
using System.Linq;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class TaskTypeReader : IPropertyReader
    {
        public TaskTypeReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.TaskType
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var taskType = data.Data.References.TaskTypes.Single(t => t.Id == item.TaskTypeId);
            cell.TaskType = new Models.TaskType
            {
                Name = taskType.Name,
                Color = WtColorHelper.GetColorByClass(taskType.Icon),
                Glyph = WtIconHelper.GetGlyph("wtf-type-" + taskType.Icon)
            };
        }
    }
}
