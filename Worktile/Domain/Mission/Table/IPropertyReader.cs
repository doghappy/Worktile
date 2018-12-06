using System.Collections.Generic;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Enums;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    interface IPropertyReader
    {
        List<WtTaskPropertyType> SupportTypes { get; }

        void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data);
    }
}
