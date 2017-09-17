using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat.Enterprise
{
    public class WeChatException : Exception
    {
        public int ErrorCode { get; private set; }

        public string ErrorMessage { get; private set; }

        public WeChatException(int errorCode, string errorMsg) : base()
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMsg;
        }

        public WeChatException(int errorCode, string errorMsg, string message) : base(message)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMsg;
        }
        public override string ToString()
        {
            return $"ErrorCode={ErrorCode}, ErrorMessage={ErrorMessage}, {base.ToString()}";
        }
    }
}
