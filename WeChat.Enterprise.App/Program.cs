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
            //var result = weChat.UploadMediaAsync(key, @"C:\Users\hoze\Pictures\200826114338-10831.jpg").Result;
            var targets = new MessageSendTargets().ChangeAll(true);
            //await weChat.CreateMaterialSender().WithFile(@"C:\Users\hoze\Videos\lxy.mp4")
            //       .SetVideoParams("刘心依", "刘心依的小视频").SendAsync(key, targets);
            //await weChat.CreateMaterialSender().WithFile(@"C:\Users\hoze\Videos\lxy.wav")
            //       .SetVideoParams("刘心依", "刘心依的小视频").SendAsync(key, targets);
            //await weChat.CreateMaterialSender().WithFile(@"C:\Users\hoze\Videos\lxy.png")
            //       .SetVideoParams("刘心依", "刘心依的小视频").SendAsync(key, targets);
            //await weChat.CreateMaterialSender().WithFile(@"C:\Users\hoze\Videos\lxy.amr")
            //       .SetVideoParams("刘心依", "刘心依的小视频").SendAsync(key, targets);
            //Task.Delay(7200 * 1000).Wait();
            //var matrial = weChat.DownloadMediaAsync(key, result.MediaId).Result;
            //var agent = weChat.GetAgentAsync(key).Result;

            //await new TextCardMessageSender(weChat).Title("领奖通知").Gary("2016年9月26日").Normal("这就是一条测试信息，请不要当真啊")
            //    .Highlight("不是倒车请注意！").Url("http://baidu.com").SendAsync(key, targets);
            //await weChat.CreateNewsMessageSender().Title("刘心依的照片片").Description("刘心依美美的照片片").PictureUrl("http://img3.utuku.china.com/650x0/ent/20170911/0dc025fe-9daf-4baf-8c9f-5d9786181996.jpg").Url("http://baidu.com").SendAsync(key, targets);

            await weChat.CreateMpNewsMessageSender()
                .Title("刘心依的照片片")
                .Digest("刘心依美美的照片片")
                .ContentUrl("http://baidu.com")
                .Content("不知道啊")
                .Anthor("刘浩")
                .WithFile(@"C:\Users\hoze\Pictures\200826114338-10831.jpg").SendAsync(key, targets);
        }
    }
}
