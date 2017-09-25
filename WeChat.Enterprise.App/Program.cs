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
            await weChat.CreateMessage<TextMessage>().Append("AAAAA").SendAsync(key, targets);

            await weChat.CreateMessage<TextCardMessage>()
                .SetTitle("领奖通知")
                .Gary("2016年9月26日")
                .Normal("恭喜你抽中iPhone 7一台，领奖码：xxxx")
                .Highlight("请于2016年10月10日前联系行政同事领取")
                .Url("URL")
                .BtnText("更多").SendAsync(key, targets);
            await weChat.CreateMessage<NewsMessage<NewsArticle>>()
                .SetMessageType(MessageTypes.News)
                .Append(new NewsArticle()
                {
                    Title = "中秋节礼品领取",
                    Description = "今年中秋节公司有豪礼相送",
                    PictureUrl = "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png",
                    Url = "null",
                    BtnTxt = "更多"

                }, new NewsArticle()
                {
                    Title = "中秋节礼品领取",
                    Description = "今年中秋节公司有豪礼相送",
                    PictureUrl = "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png",
                    Url = "null",
                    BtnTxt = "更多"

                }, new NewsArticle()
                {
                    Title = "中秋节礼品领取",
                    Description = "今年中秋节公司有豪礼相送",
                    PictureUrl = "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png",
                    Url = "null",
                    BtnTxt = "更多"

                }, new NewsArticle()
                {
                    Title = "中秋节礼品领取",
                    Description = "今年中秋节公司有豪礼相送",
                    PictureUrl = "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png",
                    Url = "null",
                    BtnTxt = "更多"

                }, new NewsArticle()
                {
                    Title = "中秋节礼品领取",
                    Description = "今年中秋节公司有豪礼相送",
                    PictureUrl = "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png",
                    Url = "null",
                    BtnTxt = "更多"

                }, new NewsArticle()
                {
                    Title = "中秋节礼品领取",
                    Description = "今年中秋节公司有豪礼相送",
                    PictureUrl = "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png",
                    Url = "null",
                    BtnTxt = "更多"

                }, new NewsArticle()
                {
                    Title = "中秋节礼品领取",
                    Description = "今年中秋节公司有豪礼相送",
                    PictureUrl = "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png",
                    Url = "null",
                    BtnTxt = "更多"

                }, new NewsArticle()
                {
                    Title = "中秋节礼品领取",
                    Description = "今年中秋节公司有豪礼相送",
                    PictureUrl = "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png",
                    Url = "null",
                    BtnTxt = "更多"

                }).SendAsync(key, targets);
        }
    }
}
