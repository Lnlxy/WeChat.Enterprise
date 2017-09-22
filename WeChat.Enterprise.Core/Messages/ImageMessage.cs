using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat.Enterprise
{
    public sealed class ImageMessage : MaterialMessage
    {
        public override string MessageType => MessageTypes.Image; 
    }
}
