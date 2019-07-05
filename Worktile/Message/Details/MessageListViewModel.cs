using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WtMessage = Worktile.Message.Models;
using Worktile.Common;

namespace Worktile.Message.Details
{
    public class MessageListViewModel : BindableBase
    {
        public MessageListViewModel()
        {
            Messages = new ObservableCollection<WtMessage.Message>();
        }

        public ObservableCollection<WtMessage.Message> Messages { get; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }


        public void TestLoad()
        {
            string json = @"
{
  ""_id"": ""5c881df3dec1c81904d43f25"",
  ""from"": {
                ""type"": 2,
    ""uid"": ""5c11f69b17a48061f1b4b0f7"",
    ""avatar"": ""appstore.png"",
    ""display_name"": ""App Store重复推送""
  },
  ""to"": {
                ""type"": 1,
    ""id"": ""5ab4a4b65690df023f728c8b""
  },
  ""type"": 32,
  ""body"": {
                ""content"": """",
    ""markdown"": 0,
    ""style"": 0,
    ""attachment"": """",
    ""links"": [


    ],
    ""inline_attachment"": {
      ""fields"": [
        {
          ""title"": ""评分"",
          ""value"": ""★★★★★"",
          ""_id"": ""5c881df3dec1c81904d43f2d"",
          ""short"": 1
        },
        {
          ""title"": ""版本"",
          ""value"": ""0.13.5"",
          ""_id"": ""5c881df3dec1c81904d43f2c"",
          ""short"": 1
        }
      ],
      ""text"": ""好了"",
      ""title_link"": ""https://itunes.apple.com/cn/review?id=1321803705&type=Purple%20Software"",
      ""title"": ""hao"",
      ""author_icon"": """",
      ""author_link"": """",
      ""author_name"": """",
      ""pretext"": ""[https://itunes.apple.com/cn/reviews/id555041032|大兵艾德曼] 评论了 绝地求生:刺激战场"",
      ""color"": ""#3da553"",
      ""fallback"": ""大兵艾德曼 评论了 绝地求生:刺激战场""
    }
  },
  ""client"": 13,
  ""created_at"": 1552424435,
  ""team"": ""567a56cc59d9e1c537c83b9a"",
  ""is_star"": 0,
  ""is_pinned"": 0
}
";
            var msg = JObject.Parse(json).ToObject<WtMessage.Message>();
            Messages.Add(msg);
        }
    }
}
