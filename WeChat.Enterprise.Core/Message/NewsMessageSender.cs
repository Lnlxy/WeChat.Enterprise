using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeChat.Enterprise
{
    public sealed class NewsMessageSender : MessageSender
    {
        private string _title, _description, _url, _picUrl, _btnTxt;
        public NewsMessageSender(WeChat weChat) : base(weChat)
        {

        }

        public NewsMessageSender Title(string title)
        {
            _title = title;
            return this;
        }
        public NewsMessageSender Description(string description)
        {
            _description = description;
            return this;
        }
        public NewsMessageSender Url(string url)
        {
            _url = url;
            return this;
        }

        public NewsMessageSender PictureUrl(string picUrl)
        {
            _picUrl = picUrl;
            return this;
        }
        public NewsMessageSender BtnTxt(string btntxt)
        {
            _btnTxt = btntxt;
            return this;
        }

        protected override Task GetMessageContentAsync(JObject content, AgentKey agentKey)
        {
            return Task.Run(() =>
            {
                var cont = new JObject
                {
                    {
                        "articles",
                        new JArray(new JObject()
                        {
                            { "title", _title },
                            { "description", _description },
                            { "url", _url },
                            { "picurl", _picUrl },
                            { "btntxt", _btnTxt }
                        })
                    },
                };
                content.Add("msgtype", "news");
                content.Add("news", cont);
            });
        }
    }
}
