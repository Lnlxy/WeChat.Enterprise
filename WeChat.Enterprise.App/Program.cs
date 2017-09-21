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
            //var result = weChat.UploadMediaAsync(key, @"C:\Users\hoze\Pictures\200826114338-10831.jpg").Result;
            var targets = new MessageSendTargets().ChangeAll(true);
            await weChat.CreateTextSender()
                .AppendLine("你好，你好啊，请看下面的啊")
                .Append("这是一个测试的啊")
                .AppendLine()
                .AppendLink("百度一下，你就知道", "http://www.baidu.com").SendAsync(key, targets);
            //Task.Delay(7200 * 1000).Wait();
            //var matrial = weChat.DownloadMediaAsync(key, result.MediaId).Result;
            //var agent = weChat.GetAgentAsync(key).Result;
        }
    }
}
