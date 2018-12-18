using System;
using System.Collections.Generic;
using System.Linq;
using Worktile.Views.Mission.Project.Detail;
using Newtonsoft.Json.Linq;
using Worktile.Enums;
using Worktile.WtRequestClient;
using System.Threading.Tasks;
using Worktile.ApiModels.ApiMissionVnexTaskProperties;
using Data = Worktile.ApiModel.ApiMissionVnextTask.Data;

namespace Worktile.Domain.Mission.Info
{
    class PropertiesReader
    {
        public PropertiesReader()
        {
            _ignorePropertyKeys = new[] {
                "identifier",
                "title",
                "task_state_id",
                "task_type_id",
                "created_by",
                "created_at",
                "updated_at",
                "updated_at",
                "assignee",
                "start",
                "due",
                "attachment",
                "child",
                "workload",
                "relation"
            };
            _hasOptionControls = new[] { "Priority", "ComboBox" };
        }

        readonly string[] _ignorePropertyKeys;
        readonly string[] _hasOptionControls;

        public IEnumerable<PropertyItem> Read(Data task)
        {
            foreach (var prop in task.References.Properties)
            {
                if (!_ignorePropertyKeys.Contains(prop.RawKey))
                {
                    var item = new PropertyItem
                    {
                        Header = prop.Name,
                    };
                    JObject jObj = JObject.FromObject(task.Value);
                    IPropertyReader reader = null;
                    switch (prop.Type)
                    {
                        case WtTaskPropertyType.Number:
                            reader = new TextBoxReader();
                            break;
                        case WtTaskPropertyType.Priority:
                            reader = new PriorityReader();
                            break;
                        case WtTaskPropertyType.Iteration:
                            reader = new ComboBoxReader();
                            break;
                    }
                    if (reader != null)
                    {
                        reader.Read(prop, item, jObj, task);
                        yield return item;
                    }
                }
            }
        }

        public async Task LoadOptionsAsync(string taskId, IEnumerable<PropertyItem> properties)
        {
            var client = new WtHttpClient();
            foreach (var property in properties)
            {
                if (_hasOptionControls.Contains(property.Control))
                {
                    string uri = $"api/mission-vnext/tasks/{taskId}/properties/{property.Value}/options";
                    var data = await client.GetAsync<ApiMissionVnexTaskProperties>(uri);
                    var dataSource = new List<DropdownItem>();
                    IPropertyReader reader = null;
                    switch (property.Control)
                    {
                        case "Priority":
                            reader = new PriorityReader();
                            break;
                        case "ComboBox":
                            reader = new ComboBoxReader();
                            break;
                    }
                    if (reader != null)
                    {
                        reader.LoadOptions(property, data.Data.Value);
                    }
                }
            }
        }
    }
}
