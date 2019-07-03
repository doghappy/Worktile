using System;

namespace Worktile.Domain.SocketMessageConverter
{
    class UnkonwMessageTypeException : InvalidCastException
    {
        public override string Message => "未知的消息类型。如果消息需要被解析，请实现\"ISocketConverter\"接口。";
    }
}
