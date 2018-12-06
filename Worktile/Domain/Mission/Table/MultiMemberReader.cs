using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class MultiMemberReader : IPropertyReader
    {
        public MultiMemberReader()
        {
            SupportTypes = new List<WtTaskPropertyType>
            {
                WtTaskPropertyType.MultiMember
            };
        }

        public List<WtTaskPropertyType> SupportTypes { get; }

        public void Read(Cell cell, ValueElement item, HeaderCell column, ApiMissionVnextTableContent data)
        {
            var jObj = JObject.FromObject(item);
            string[] uids = TaskHelper.GetPropertyValue<string[]>(jObj, column.Key);
            if (uids.Length > 0)
            {
                cell.Avatars = new List<Avatar>();
                foreach (var uid in uids)
                {
                    var avatar = CommonData.GetAvatar(uid, 40);
                    cell.Avatars.Add(avatar);
                }
            }
        }
    }
}
