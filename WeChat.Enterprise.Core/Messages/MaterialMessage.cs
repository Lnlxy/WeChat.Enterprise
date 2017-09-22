using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeChat.Enterprise
{
    /// <summary>
    /// 定义媒体消息信息。
    /// </summary>
    public abstract class MaterialMessage : Message
    {
        private Func<AgentKey, Task<string>> getMaterialFunc;

        public MaterialMessage WithFile(string fileName)
        {
            getMaterialFunc = new Func<AgentKey, Task<Material>>(async i => await WeChat.MaterialCache.Create(fileName, i));
            return this;
        }

        public MaterialMessage WithMediaId(string mediaId)
        {
            getMaterialFunc = new Func<AgentKey, Task<Material>>(i => Task.Run(() => WeChat.MaterialCache.Get(mediaId)));
            return this;
        }

        protected override Task GetMessageContentAsync(JObject content, AgentKey agentKey)
        {
            return Task.Run(async () =>
            {
                var mediaId = await getMaterialFunc.Invoke(agentKey);
                var cont = new JObject();
                cont.Add("media_id", mediaId);
                content.Add(MessageType, cont);
            });
        }
    }
}
