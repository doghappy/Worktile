using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Worktile.WebUI.Models;
using SocketIO = Quobject.SocketIoClientDotNet.Client;
using WtMessage = Worktile.WebUI.Models.Message;

namespace Worktile.WebUI.Common
{
    public static class WtSocketClient
    {
        private static SocketIO.Socket _socket;

        public static event Action<JObject> OnMessageReceived;

        public static void Connect(string imHost, string imToken)
        {
            _socket = SocketIO.IO.Socket(imHost, new SocketIO.IO.Options
            {
                Transports = Quobject.Collections.Immutable.ImmutableList.Create<string>().Add("websocket"),
                Query = new Dictionary<string, string>
                {
                    { "token", imToken },
                    { "uid", MainPage.Me.Id },
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
                from = MainPage.Me.Id,
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
