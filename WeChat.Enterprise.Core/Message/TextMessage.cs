using Newtonsoft.Json.Linq;

namespace WeChat.Enterprise
{
    /// <summary>
    /// 定义文本消息格式。
    /// </summary>
    public sealed class TextMessage : Message, IMessage
    {
        /// <summary>
        /// 获取一个值，该值表示消息格式。
        /// </summary>
        public override string MessageType => MessageTypes.Text;

        /// <summary>
        /// 获取一个值，该值表示文本内容。
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// 初始化 <see cref="TextMessage"/> 新实例。
        /// </summary>
        /// <param name="text">文本内容。</param>
        public TextMessage(string text)
        {
            Text = text;
        }

        protected override void GetContentExtra(JObject content)
        {
            content.Add("text", new JProperty("content", Text));
        }
    }
}
