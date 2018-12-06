using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Mission;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class WorkloadReader : IPropertyReader
    {
        public WorkloadReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.Workload
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var jObj = JObject.FromObject(item);
            var workload = TaskHelper.GetPropertyValue<WorkloadValue>(jObj, column.Key);
            cell.Expected = workload.Estimated.Duration;
            cell.Actual = workload.Actual;
        }
    }
}
