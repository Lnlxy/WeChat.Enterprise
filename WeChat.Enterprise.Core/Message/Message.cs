using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeChat.Enterprise
{
    public abstract class Message : IMessage
    {
        private readonly MessageTargets targets = new MessageTargets();

        public abstract string MessageType { get; }

        public bool IsSafe { get; set; }

        public MessageTargets Targets => targets;
        
        public string GetContent()
        {
            var jObject = CreateBaseJObject();
            GetContentExtra(jObject);
            return jObject.ToString(Formatting.None);
        }

        private JObject CreateBaseJObject()
        {
            JObject jObject = new JObject();
            targets.AppendToObject(jObject);
            jObject.Add("msgtype", MessageType);
            jObject.Add("safe", IsSafe ? 1 : 0);
            return jObject;
        }

        protected abstract void GetContentExtra(JObject content);
    }
}
