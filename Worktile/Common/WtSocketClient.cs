using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Worktile.Main;
using Worktile.Models;
using SocketIO = Quobject.SocketIoClientDotNet.Client;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Common
{
    public static class WtSocketClient
    {
        private static SocketIO.Socket _socket;

        public static event Action<JObject> OnMessageReceived;

        public static void Connect(string imHost, string imToken, string uid)
        {
            _socket = SocketIO.IO.Socket(imHost, new SocketIO.IO.Options
            {
                Transports = Quobject.Collections.Immutable.ImmutableList.Create<string>().Add("websocket"),
                Query = new Dictionary<string, string>
                {
                    { "token", imToken },
                    { "uid", uid },
                    { "client", "Windows 10" },
                }
            });
            _socket.On("message", obj => OnMessageReceived?.Invoke(obj as JObject));
        }

        public static void SendMessage(string to, WtMessage.ToType toType, string msg, WtMessage.MessageType messageType)
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
            _socket.Emit("message", obj);
        }
    }
}
