using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat.Enterprise
{
    public sealed class FileMessage : Message
    {
        public override string MessageType => MessageTypes.File;

        protected override void GetContentExtra(JObject content)
        {
            throw new NotImplementedException();
        }
    }
}
