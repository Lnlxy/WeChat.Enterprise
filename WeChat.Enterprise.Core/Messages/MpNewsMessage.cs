using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeChat.Enterprise
{
    public sealed class MpNewsMessage : Message
    {
        private Func<AgentKey, Task<Material>> getMaterialFunc;
        private string _anthor, _title, _digest, _url, _btnTxt;
        private string _content;

        public override string MessageType => MessageTypes.Mpnews; 
        public MpNewsMessage Anthor(string anthor)
        {
            _anthor = anthor;
            return this;
        }
        public MpNewsMessage Content(string content)
        {
            _content = content;
            return this;
        }
        public MpNewsMessage Title(string title)
        {
            _title = title;
            return this;
        }
        public MpNewsMessage Digest(string digest)
        {
            _digest = digest;
            return this;
        }
        public MpNewsMessage ContentUrl(string url)
        {
            _url = url;
            return this;
        }

        public MpNewsMessage With(Material material)
        {
            getMaterialFunc = new Func<AgentKey, Task<Material>>(i => Task.Run(() => material));
            return this;
        }

        public MpNewsMessage WithFile(string fileName)
        {
            getMaterialFunc = new Func<AgentKey, Task<Material>>(async i => await WeChat.MaterialCache.Create(fileName, i));
            return this;
        }

        public MpNewsMessage WithMediaId(string mediaId)
        {
            getMaterialFunc = new Func<AgentKey, Task<Material>>(i => Task.Run(() => WeChat.MaterialCache.Get(mediaId)));
            return this;
        }

        public MpNewsMessage BtnTxt(string btntxt)
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
