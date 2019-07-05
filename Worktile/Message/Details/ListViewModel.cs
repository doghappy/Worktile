//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WtMessage = Worktile.Message.Models;
//using Worktile.Common;
//using Worktile.Models;

//namespace Worktile.Message.Details
//{
//    public abstract class ListViewModel : BindableBase
//    {
//        public ListViewModel()
//        {
//            Messages = new ObservableCollection<WtMessage.Message>();
//            HasMore = true;
//            PageSize = 20;
//        }

//        public ObservableCollection<WtMessage.Message> Messages { get; }

//        protected int PageSize { get; }

//        public bool HasMore { get; private set; }

//        public Session Session { get; set; }

//        private bool _isActive;
//        public bool IsActive
//        {
//            get => _isActive;
//            set => SetProperty(ref _isActive, value);
//        }

//        protected string LatestId { get; set; }

//        public async Task LoadMessagesAsync()
//        {
//            if (HasMore)
//            {
//                IsActive = true;
//                string url = $"api/messages?ref_id={Session.Id}&ref_type={Session.RefType}&latest_id={LatestId}&size={PageSize}";
//                var obj = await WtHttpClient.GetAsync(url);
//                if (LatestId != null)
//                {
//                    HasMore = obj["data"].Value<bool>("more");
//                }
//                LatestId = obj["data"].Value<string>("next");
//                var msgs = obj["data"]["messages"].Children<JObject>().OrderByDescending(g => g.Value<double>("created_at"));
//                foreach (var item in msgs)
//                {
//                    Messages.Insert(0, item.ToObject<WtMessage.Message>());
//                }
//                IsActive = false;
//            }
//        }
//    }
//}
