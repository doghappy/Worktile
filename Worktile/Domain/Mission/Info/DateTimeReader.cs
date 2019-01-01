using Newtonsoft.Json.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.ApiModel.ApiMissionVnextTask;
using Worktile.Common;
using System.Collections.Generic;
using Windows.UI.Core;
using System;
using System.Threading.Tasks;
using Worktile.Models;

namespace Worktile.Domain.Mission.Info
{
    class DateTimeReader : IPropertyReader
    {
        public string Control => "DateTime";

        public void Read(Property property, PropertyItem item, Data data)
        {
            JObject task = JObject.FromObject(data.Value);
            var wtDate = TaskHelper.GetPropertyValue<WtDate>(task, property.Key);
            item.TimeSpan = new TimeSpan();
            if (wtDate != null && wtDate.Date.HasValue)
            {
                item.Date = new DateTime(wtDate.Date.Value.Year, wtDate.Date.Value.Month, wtDate.Date.Value.Day);
                if (wtDate.WithTime)
                {
                    item.TimeSpan = wtDate.Date.Value - item.Date.Value;
                }
            }

        }

        public Task LoadOptionsAsync(PropertyItem item, List<JObject> allProps, CoreDispatcher dispatcher)
        {
            throw new NotImplementedException();
        }
    }
}
