using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl;
namespace WeChat.Enterprise
{
    public class MessageSender<T> where T : IMessage
    {
        private readonly WeChat weChat;

        public MessageSender(WeChat weChat)
        {
            this.weChat = weChat;
        }

        public Task<MessageSendResult> SendAsync(AgentKey agent)
        {
            return null; 


        }
    }
}
