using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.ApiModel.ApiMissionVnextTask;
using System.Collections.Generic;

namespace Worktile.Domain.Mission.Info
{
    interface IPropertyReader
    {
        void Read(Property property, PropertyItem item, JObject task, Data data);
        void LoadOptions(PropertyItem item, List<JObject> allProps);
    }
}
