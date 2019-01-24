using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.ApiModel.ApiMissionVnextTask;
using Worktile.Common;
using System.Collections.Generic;
using Windows.UI.Core;
using System;
using System.Threading.Tasks;
using Worktile.Models;
using Worktile.Infrastructure;

namespace Worktile.Domain.Mission.Info
{
    class DateSpanReader : IPropertyReader
    {
        public string Control => "DateSpan";

        public void Read(Property property, PropertyItem item, Data data)
        {
            JObject task = JObject.FromObject(data.Value);
            var beginEnd = JTokenHelper.GetPropertyValue<BeginEnd>(task, property.Key);
            if (beginEnd.Begin.Date != null && beginEnd.Begin.Date.HasValue)
            {
                item.Begin = beginEnd.Begin.Date.Value;
                item.End = beginEnd.End.Date.Value;
            }

        }

        public Task LoadOptionsAsync(PropertyItem item, List<JObject> allProps, CoreDispatcher dispatcher)
        {
            throw new NotImplementedException();
        }
    }
}
