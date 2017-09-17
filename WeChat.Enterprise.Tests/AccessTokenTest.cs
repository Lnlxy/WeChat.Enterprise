using System;
using Xunit;

namespace WeChat.Enterprise.Tests
{
    public class AccessTokenTest
    {
        [Xunit.Theory()]
        [Xunit.InlineData("ww8426c9dcc43f09a0", 1000004, "VZfBNQXBQCSj1GY-Sipurf-Qm8IKC1Rtw05R2gre6N4")]
        public void CreateAccessToken(string corpId, int agentId, string secret)
        {
            WeChat weChat = new WeChat(corpId);

        }
    }
}
