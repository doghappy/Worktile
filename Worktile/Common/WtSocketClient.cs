using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using SocketIO = Quobject.SocketIoClientDotNet.Client.IO;

namespace Worktile.Common
{
    public static class WtSocketClient
    {
        public static event Action<JObject> OnMessageReceived;

        public static void Connect(string imHost, string imToken, string uid)
        {
            var socket = SocketIO.Socket(imHost, new SocketIO.Options
            {
                Transports = Quobject.Collections.Immutable.ImmutableList.Create<string>().Add("websocket"),
                Query = new Dictionary<string, string>
                {
                    { "token", imToken },
                    { "uid", uid },
                    { "client", "Windows 10" },
                }
            });
            socket.On("message", obj => OnMessageReceived?.Invoke(obj as JObject));
        }
    }
}
