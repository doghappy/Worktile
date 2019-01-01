using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.ApiModel.ApiMissionVnextTask;
using System.Collections.Generic;
using Windows.UI.Core;
using System.Threading.Tasks;

namespace Worktile.Domain.Mission.Info
{
    interface IPropertyReader
    {
        string Control { get; }
        void Read(Property property, PropertyItem item, Data data);
        Task LoadOptionsAsync(PropertyItem item, List<JObject> allProps, CoreDispatcher dispatcher);
    }
}
