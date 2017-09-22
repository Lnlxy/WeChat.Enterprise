using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeChat.Enterprise
{
    public sealed class VideoMessage : MaterialMessage
    {
        private string _title, _description;
        public override string MessageType => MessageTypes.Video; 
        protected override Task GetMessageContentAsync(JObject content, AgentKey agentKey)
        {
            return base.GetMessageContentAsync(content, agentKey).ContinueWith(i =>
            {
                var cont = (JObject)content[MessageType];
                cont.Add("title", _title);
                cont.Add("description", _description);
            });
        }
    }
}
