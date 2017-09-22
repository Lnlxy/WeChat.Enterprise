using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeChat.Enterprise
{
    public sealed class NewsMessage : Message
    {
        private string _title, _description, _url, _picUrl, _btnTxt;

        public override string MessageType => MessageTypes.News; 
        public NewsMessage Title(string title)
        {
            _title = title;
            return this;
        }
        public NewsMessage Description(string description)
        {
            _description = description;
            return this;
        }
        public NewsMessage Url(string url)
        {
            _url = url;
            return this;
        }

        public NewsMessage PictureUrl(string picUrl)
        {
            _picUrl = picUrl;
            return this;
        }
        public NewsMessage BtnTxt(string btntxt)
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
