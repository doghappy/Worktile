using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class MemberReader : IPropertyReader
    {
        public MemberReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.Member
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var jObj = JObject.FromObject(item);
            string value = TaskHelper.GetPropertyValue<string>(jObj, column.Key);
            if (value != null)
            {
                cell.Avatar = CommonData.GetAvatar(value, 40);
            }
        }
    }
}
