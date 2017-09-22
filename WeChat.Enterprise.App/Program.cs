using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeChat.Enterprise.App
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>(100);
            CreateAccessTokenAsync("ww8426c9dcc43f09a0", 1000004, "VZfBNQXBQCSj1GY-Sipurf-Qm8IKC1Rtw05R2gre6N4").Wait();
        }
        public static async Task CreateAccessTokenAsync(string corpId, int agentId, string secret)
        {
            var key = new AgentKey(agentId, secret);
            WeChat weChat = new WeChat(corpId);

            var targets = new MessageSendTargets().ChangeAll(true);
            var message = weChat.CreateMessage<ImageMessage>().WithFile(@"C:\Users\hoze\Pictures\140-1506050ZA6.jpg");
            while (true)
            {
                await message.SendAsync(key, targets);
            }
        }
    }
}
