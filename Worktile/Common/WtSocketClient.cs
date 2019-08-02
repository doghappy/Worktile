using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Worktile.Main;
using Worktile.Models;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Common
{
    public static class WtSocketClient
    {
        private static SocketIO _socket;

        public static event Action<JObject> OnMessageReceived;

        public static async Task ConnectAsync(string imHost, string imToken, string uid)
        {
            _socket = new SocketIO(imHost)
            {
                Parameters = new Dictionary<string, string>
                {
                    { "token", imToken },
                    { "uid", uid },
                    { "client", "Windows 10" }
                }
            };
            _socket.OnOpened += async args =>
            {
                string text = "40" + _socket.Namespace;
                text = text.Insert(text.Length - 1, $"?token={imToken}&uid={uid}&client=web");
                await _socket.SendMessageAsync(text);
            };
            await _socket.ConnectAsync();
            _socket.On("message", args =>
            {
                var jobj = JObject.Parse(args.Text);
                OnMessageReceived?.Invoke(jobj);
            });
        }

        public static async Task SendMessageAsync(string to, WtMessage.ToType toType, string msg, WtMessage.MessageType messageType)
        {
            var obj = JObject.FromObject(new
            {
                fromType = WtMessage.FromType.User,
                from = MainViewModel.Me.Id,
                to,
                toType,
                messageType,
                client = Client.Win8,
                markdown = 1,
                content = msg
            });
            await _socket.EmitAsync("message", obj);
        }
    }
}
