using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace WeChat.Enterprise
{
    public sealed class ImageMessage : Message, IMessage
    {
        private Task<string> imageIdTask;
        public override string MessageType => MessageTypes.Image;

        public ImageMessage SetImageId(string imageId)
        {
            imageIdTask = new Task<string>(() => imageId);
            return this;
        }
        
        protected override void GetContentExtra(JObject content)
        {
            content.Add("image",new JProperty("media_id",))
        }
    }
}
