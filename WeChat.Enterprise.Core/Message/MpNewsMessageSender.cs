using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeChat.Enterprise
{
    public sealed class MpNewsMessageSender : MessageSender
    {
        private Func<AgentKey, Task<Material>> getMaterialFunc;
        private string _anthor, _title, _digest, _url, _btnTxt;
        private string _content;
        public MpNewsMessageSender(WeChat weChat) : base(weChat)
        {
        }
        public MpNewsMessageSender Anthor(string anthor)
        {
            _anthor = anthor;
            return this;
        }
        public MpNewsMessageSender Content(string content)
        {
            _content = content;
            return this;
        }
        public MpNewsMessageSender Title(string title)
        {
            _title = title;
            return this;
        }
        public MpNewsMessageSender Digest(string digest)
        {
            _digest = digest;
            return this;
        }
        public MpNewsMessageSender ContentUrl(string url)
        {
            _url = url;
            return this;
        }

        public MpNewsMessageSender With(Material material)
        {
            getMaterialFunc = new Func<AgentKey, Task<Material>>(i => Task.Run(() => material));
            return this;
        }

        public MpNewsMessageSender WithFile(string fileName)
        {
            getMaterialFunc = new Func<AgentKey, Task<Material>>(async i => await WeChat.MaterialCache.Create(fileName, i));
            return this;
        }

        public MpNewsMessageSender WithMediaId(string mediaId)
        {
            getMaterialFunc = new Func<AgentKey, Task<Material>>(i => Task.Run(() => WeChat.MaterialCache.Get(mediaId)));
            return this;
        }

        public MpNewsMessageSender BtnTxt(string btntxt)
        {
            _btnTxt = btntxt;
            return this;
        }

        protected override Task GetMessageContentAsync(JObject content, AgentKey agentKey)
        {
            return Task.Run(async () =>
            {
                var material = await getMaterialFunc.Invoke(agentKey);
                var cont = new JObject
                {
                    {
                        "articles",
                        new JArray(new JObject()
                        {
                            { "title", _title },
                            { "thumb_media_id", material.MediaId },
                            { "author",_anthor},
                            { "content_source_url", _url },
                            { "content", _content  },
                            { "digest", _digest },
                        })
                    },
                };
                content.Add("msgtype", "mpnews");
                content.Add("mpnews", cont);
            });
        }
    }
}
