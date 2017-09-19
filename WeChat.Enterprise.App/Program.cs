using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeChat.Enterprise.App
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>(100);
            CreateAccessToken("ww8426c9dcc43f09a0", 1000004, "VZfBNQXBQCSj1GY-Sipurf-Qm8IKC1Rtw05R2gre6N4");
        }
        public static void CreateAccessToken(string corpId, int agentId, string secret)
        {

            WeChat weChat = new WeChat(corpId);
            var key = new AgentKey(agentId, secret);
            var result = weChat.UploadMediaAsync(key, @"C:\Users\hoze\Pictures\200826114338-10831.jpg").Result;

        }
    }
}
