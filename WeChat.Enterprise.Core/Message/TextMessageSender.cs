using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Enterprise
{
    public sealed class TextMessageSender : MessageSender
    {
        private readonly StringBuilder sb = new StringBuilder();

        public TextMessageSender(WeChat weChat) : base(weChat)
        {
        }

        public override string MessageType => MessageTypes.Text;

        public TextMessageSender ClearText()
        {
            sb.Clear();
            return this;
        }
        public TextMessageSender ClearNew(string newText)
        {
            sb.Clear();
            sb.Append(newText);
            return this;
        }

        public TextMessageSender Append(string value)
        {
            sb.Append(value);
            return this;
        }
        public TextMessageSender AppendLine()
        {
            sb.AppendLine();
            return this;
        }
        public TextMessageSender AppendLine(string value)
        {
            sb.AppendLine(value);
            return this;
        }

        public TextMessageSender AppendLink(string name, string url)
        {
            sb.AppendFormat("<a href=\"{0}\">{1}</a>", url, name);
            return this;
        }

        public TextMessageSender AppendJion(string separator, params object[] values)
        {
            sb.AppendJoin(separator, values);
            return this;
        }
        public TextMessageSender AppendJion<T>(string separator, IEnumerable<T> values)
        {
            sb.AppendJoin(separator, values);
            return this;
        }


        protected override Task GetMessageContentAsync(JObject content, AgentKey agentKey)
        {
            return Task.Run(() =>
            {
                var cont = new JObject();
                cont.Add("content", sb.ToString());
                content.Add("text", cont);
            });
        }
    }
}
