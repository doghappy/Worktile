using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Enums;
using Worktile.Models.Mission.Table;

namespace Worktile.Domain.Mission.Table
{
    class PropertiesReader
    {
        public PropertiesReader()
        {
            _readers = new List<IPropertyReader>
            {
                new AttachmentReader(),
                new DateSpanReader(),
                new DateTimeReader(),
                new MemberReader(),
                new MultiMemberReader(),
                new PriorityReader(),
                new TagReader(),
                new TaskStateReader(),
                new TaskTypeReader(),
                new TextReader(),
                new WorkloadReader(),
                new MultiSelectReader(),
                new DefaultReader()
            };
        }

        private readonly List<IPropertyReader> _readers;

        public List<List<Cell>> Read(string idPrefix, ApiMissionVnextTableContent data, IEnumerable<HeaderCell> headerCells)
        {
            var rows = new List<List<Cell>>();
            int no = 0;
            foreach (var item in data.Data.Value)
            {
                no++;
                var row = new List<Cell>
                {
                    new Cell
                    {
                        Text = no.ToString(),
                        Width = 80,
                        RenderParam = nameof(DefaultReader)
                    }
                };
                foreach (var column in headerCells)
                {
                    var cell = new Cell
                    {
                        Width = column.Width,
                        TaskId = item.Id
                    };
                    if (column.RowKey == "identifier")
                    {
                        cell.RenderParam = "Identifier";
                        cell.Text = idPrefix + item.Identifier;
                        cell.Foreground = new SolidColorBrush(Color.FromArgb(255, 34, 215, 187));
                        cell.Background = new SolidColorBrush(Color.FromArgb(26, 34, 215, 187));
                    }
                    else
                    {
                        var reader = _readers.Single(r => r.SupportTypes.Contains(column.Type));
                        cell.RenderParam = reader.GetType().Name;
                        reader.Read(cell, item, column, data);
                    }
                    row.Add(cell);
                }
                rows.Add(row);
            }
            return rows;
        }
    }
}
