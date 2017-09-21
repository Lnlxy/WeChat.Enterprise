using Flurl.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace WeChat.Enterprise
{
    public class MaterialCache
    {
        private readonly WeChat _weChat;
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        public MaterialCache(WeChat weChat)
        {
            _weChat = weChat;
        }

        /// <summary>
        /// 获取或创建一个新的素材。
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public Task<Material> GetOrCreateAsync(string mediaId, string file, AgentKey agentKey)
        {
            return Task.Run(async () =>
            {
                if (!_cache.TryGetValue(mediaId, out Material material))
                {
                    material = await Material.LoadFromAsync(file);
                    await UploadMediaAsync(agentKey, material);
                    _cache.Set(material.MediaId, material, new TimeSpan(3 * 24, 0, 0));
                }
                return material;
            });
        }

        public Material Get(string mediaId)
        {
            _cache.TryGetValue(mediaId, out Material material);
            return material;
        }

        public Task<Material> Create(string file, AgentKey agentKey)
        {
            return Task.Run(async () =>
            {
                var material = await Material.LoadFromAsync(file);
                await UploadMediaAsync(agentKey, material);
                _cache.Set(material.MediaId, material, new TimeSpan(3 * 24, 0, 0));
                return material;
            });
        }

        /// <summary>
        /// 上传临时文件。
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task UploadMediaAsync(AgentKey agent, Material material)
        {
            return Task.Run(async () =>
            {
                var token = await _weChat.GetAccessTokenAsync(agent);
                var url = _weChat.GetAccessDomainUrl()
                    .AppendPathSegment("media")
                    .AppendPathSegment("upload")
                    .SetQueryParam("type", material.MediaType);
                ree:
                var result = await url.SetQueryParam("access_token", token)
                    .PostAsync(material.CreateMultipartFormDataContent())
                    .ReceiveJson();
                var errorCode = (int)result.errcode;
                if (_weChat.NeedRefreshAccessToken(errorCode))
                {
                    token = await _weChat.GetAccessTokenAsync(agent, true);
                    goto ree;
                }
                if (errorCode != 0)
                {
                    throw new WeChatException(errorCode, (string)result.errmsg);
                }
                material.MediaId = (string)result.media_id;
            });
        }

        public Task<Material> DownloadMediaAsync(AgentKey key, string mediaId)
        {
            //https://qyapi.weixin.qq.com/cgi-bin/media/get?access_token=ACCESS_TOKEN&media_id=MEDIA_ID
            return Task.Run(async () =>
            {
                var token = await _weChat.GetAccessTokenAsync(key);
                var result = await _weChat.GetAccessDomainUrl()
                .AppendPathSegment("media")
                .AppendPathSegment("get")
                .SetQueryParams(new { access_token = token.Token, media_id = mediaId })
                .GetAsync();
                return await Material.LoadFromAsync(result.Content);
            });
        }

    }
}
