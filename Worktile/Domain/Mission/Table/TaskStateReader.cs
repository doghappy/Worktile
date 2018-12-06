using System.Collections.Generic;
using System.Linq;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class TaskStateReader : IPropertyReader
    {
        public TaskStateReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.State
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var state = data.Data.References.Lookups.TaskStates.Single(t => t.Id == item.TaskStateId);
            cell.TaskState = new Models.TaskState
            {
                Name = state.Name,
                Foreground = WtColorHelper.GetNewColor(state.Color),
                Glyph = WtIconHelper.GetGlyph(state.Type)
            };
        }
    }
}
