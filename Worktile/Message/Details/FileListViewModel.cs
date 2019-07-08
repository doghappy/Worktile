using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Models;

namespace Worktile.Message.Details
{
    public class FileListViewModel : BindableBase
    {
        public FileListViewModel()
        {
            SelectionMode = ListViewSelectionMode.Single;
        }

        public IncrementalLoadingCollection<EntitySource, Entity> Entities { get; private set; }

        Session _session;
        public Session Session
        {
            get => _session;
            set
            {
                _session = value;
                Entities = new IncrementalLoadingCollection<EntitySource, Entity>(new EntitySource(value));
            }
        }

        private ListViewSelectionMode _selectionMode;
        public ListViewSelectionMode SelectionMode
        {
            get => _selectionMode;
            set => SetProperty(ref _selectionMode, value);
        }
    }

    public class EntitySource : IIncrementalSource<Entity>
    {
        public EntitySource(Session session)
        {
            _session = session;
        }

        int _page = 1;
        Session _session;

        public async Task<IEnumerable<Entity>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            string refType = _session.Type == SessionType.Session ? "2" : "1";
            string url = $"api/entities?ref_type={refType}&ref_id={_session.Id}&page={_page}&size=20";
            var obj = await WtHttpClient.GetAsync(url);
            var entities = obj["data"]["entities"].Children<JObject>();
            var list = new List<Entity>();
            foreach (var item in entities)
            {
                list.Add(item.ToObject<Entity>());
            }
            _page++;
            return list;
        }
    }
}
