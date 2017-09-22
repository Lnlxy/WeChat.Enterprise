using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Enterprise
{
    /// <summary>
    /// 构建文本消息。
    /// </summary>
    public sealed class TextMessage : Message
    {
        private readonly StringBuilder sb = new StringBuilder();

        public override string MessageType => MessageTypes.Text;
         

        public TextMessage ClearText()
        {
            sb.Clear();
            return this;
        }
        public TextMessage ClearNew(string newText)
        {
            sb.Clear();
            sb.Append(newText);
            return this;
        }

        public TextMessage Append(string value)
        {
            sb.Append(value);
            return this;
        }
        public TextMessage AppendLine()
        {
            sb.AppendLine();
            return this;
        }
        public TextMessage AppendLine(string value)
        {
            sb.AppendLine(value);
            return this;
        }

        public TextMessage AppendLink(string name, string url)
        {
            sb.AppendFormat("<a href=\"{0}\">{1}</a>", url, name);
            return this;
        }

        public TextMessage AppendJion(string separator, params object[] values)
        {
            sb.AppendJoin(separator, values);
            return this;
        }
        public TextMessage AppendJion<T>(string separator, IEnumerable<T> values)
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
