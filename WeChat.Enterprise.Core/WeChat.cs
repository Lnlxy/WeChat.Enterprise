using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeChat.Enterprise
{
    /// <summary>
    /// 提供企业微信访问功能。
    /// </summary>
    public sealed class WeChat
    {
        private readonly AccessTokenCache accessTokenCache;
        private readonly string host = "https://qyapi.weixin.qq.com";

        public string Host => host;

        /// <summary>
        /// 获取或设置一个值，该值表示企业Id。
        /// </summary>
        public string CorpId { get; private set; }

        /// <summary>
        /// 初始化 <see cref="WeChat"/> 新实例。
        /// </summary>
        /// <param name="corpId">企业Id。</param>
        public WeChat(string corpId)
        {
            CorpId = corpId;
            accessTokenCache = new AccessTokenCache(this);
        }

        /// <summary>
        /// 创建一个消息发送器。
        /// </summary>
        /// <returns></returns>
        public MessageSender<T> CreateMessageSender<T>() where T : class, IMessage
        {
            return new MessageSender<T>(this);
        }

        public AccessToken GetAgentAccessToken(AgentKey key)
        {
            return accessTokenCache[key];
        }

        /// <summary>
        /// 获取访问域地址信息。
        /// </summary>
        /// <returns></returns>
        internal Url GetAccessDomainUrl()
        {
            return host.AppendPathSegment("cgi-bin");
        }

        /// <summary>
        /// 上传临时文件。
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public Task<UploadResult> UploadMediaAsync(AgentKey agent, string file)
        {
            return Task.Run(async () =>
            {
                var token = accessTokenCache[agent];
                var media = Material.LoadFrom(file);
                var message = await GetAccessDomainUrl()
                    .AppendPathSegment("media")
                    .AppendPathSegment("upload")
                    .SetQueryParam("access_token", token)
                    .SetQueryParam("type", media.Type)
                    .PostAsync(media.CreateMultipartFormDataContent()).ReceiveJson();
                return new UploadResult((int)message.errcode,
                    (string)message.errmsg, media.Type,
                    (string)message.media_id,
                    (string)message.created_at, media);
            });
        } 
    }
}
