using System;
using System.Collections.Generic;
using System.Linq;
using Worktile.Views.Mission.Project.Detail;
using Worktile.Enums;
using Worktile.Common.WtRequestClient;
using System.Threading.Tasks;
using Worktile.ApiModels.ApiMissionVnexTaskProperties;
using Data = Worktile.ApiModels.ApiMissionVnextTask.Data;
using Windows.UI.Core;

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
                "relation",
                "completed_at"
            };
            _hasOptionControls = new[] { "Priority", "ComboBox", "CheckBox", "Tag" };
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
                    IPropertyReader reader = null;
                    switch (prop.Type)
                    {
                        case WtTaskPropertyType.Text:
                        case WtTaskPropertyType.Number:
                            reader = new TextBoxReader();
                            break;
                        case WtTaskPropertyType.Priority:
                            reader = new PriorityReader();
                            break;
                        case WtTaskPropertyType.DropDown:
                        case WtTaskPropertyType.Iteration:
                            reader = new ComboBoxReader();
                            break;
                        case WtTaskPropertyType.MultiSelect:
                            reader = new CheckBoxReader();
                            break;
                        case WtTaskPropertyType.Tag:
                            reader = new TagReader();
                            break;
                        case WtTaskPropertyType.DateTime:
                            reader = new DateTimeReader();
                            break;
                        case WtTaskPropertyType.DateSpan:
                            reader = new DateSpanReader();
                            break;
                    }
                    if (reader != null)
                    {
                        reader.Read(prop, item, task);
                        item.Control = reader.Control;
                        yield return item;
                    }
                }
            }
        }

        public void LoadOptions(string taskId, IEnumerable<PropertyItem> properties, CoreDispatcher dispatcher)
        {
            var hasOptionsProps = properties.Where(p => _hasOptionControls.Contains(p.Control));
            Parallel.ForEach(hasOptionsProps, async property =>
            {
                string uri = $"api/mission-vnext/tasks/{taskId}/properties/{property.PropertyId}/options";
                var data = await WtHttpClient.GetAsync<ApiMissionVnexTaskProperties>(uri);
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
                    case "CheckBox":
                        reader = new CheckBoxReader();
                        break;
                    case "Tag":
                        reader = new TagReader();
                        break;
                }
                if (reader != null)
                {
                   await reader.LoadOptionsAsync(property, data.Data.Value, dispatcher);
                }
            });
        }
    }
}
