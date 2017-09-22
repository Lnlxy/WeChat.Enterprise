using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat.Enterprise
{
    public class MessageSendResult
    {
        public int ErrorCode { get; private set; }

        public string ErrorMessage { get; private set; }

        public MessageSendTargets InvalidTargets { get; private set; }

        public MessageSendResult(int errorCode, string errorMessage, MessageSendTargets targets)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            InvalidTargets = targets;
        }

        public static implicit operator bool(MessageSendResult result)
        {
            return (result?.ErrorCode ?? int.MinValue) == 0;
        }

        public static bool operator ==(bool arg1, MessageSendResult arg2)
        {
            return arg1 == arg2;
        }
        public static bool operator !=(bool arg1, MessageSendResult arg2)
        {
            return arg1 != arg2;
        }
    }
}
