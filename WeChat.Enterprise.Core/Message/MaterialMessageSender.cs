﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeChat.Enterprise
{
    public sealed class MaterialMessageSender : MessageSender
    {
        private Func<AgentKey, Task<Material>> getMaterialFunc;
        private string _name, _description;

        public MaterialMessageSender(WeChat weChat) : base(weChat)
        {
        }
        public MaterialMessageSender SetVideoParams(string name, string description)
        {
            _name = name;
            _description = description;
            return this;
        }
        public MaterialMessageSender With(Material material)
        {
            getMaterialFunc = new Func<AgentKey, Task<Material>>(i => Task.Run(() => material));
            return this;
        }

        public MaterialMessageSender WithFile(string fileName)
        {
            getMaterialFunc = new Func<AgentKey, Task<Material>>(async i => await WeChat.MaterialCache.Create(fileName, i));
            return this;
        }

        public MaterialMessageSender WithMediaId(string mediaId)
        {
            getMaterialFunc = new Func<AgentKey, Task<Material>>(i => Task.Run(() => WeChat.MaterialCache.Get(mediaId)));
            return this;
        }

        protected override Task GetMessageContentAsync(JObject content, AgentKey agentKey)
        {
            return Task.Run(async () =>
            {
                var material = await getMaterialFunc.Invoke(agentKey);
                content.Add("msgtype", MediaTypeStrings.GetMessageTypeString(material.MediaType));
                var cont = new JObject();
                content.Add("media_id", material.MediaId);
                switch (material.MediaType)
                {
                    case MediaType.Image:
                        content.Add("iamge", cont);
                        break;
                    case MediaType.Voice:
                        content.Add("voice", cont);
                        break;
                    case MediaType.Video:
                        cont.Add("title", _name);
                        cont.Add("description", _description);
                        content.Add("video", cont);
                        break;
                    case MediaType.File:
                    default:
                        content.Add("file", cont);
                        break;
                }
            });
        }
    }
}