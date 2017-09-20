using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System;
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

        public Task<AccessToken> GetAccessTokenAsync(AgentKey key)
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
                var token = await GetAccessTokenAsync(agent);
                var media = Material.LoadFrom(file);
                var result = await GetAccessDomainUrl()
                    .AppendPathSegment("media")
                    .AppendPathSegment("upload")
                    .SetQueryParam("access_token", token)
                    .SetQueryParam("type", media.Type)
                    .PostAsync(media.CreateMultipartFormDataContent()).ReceiveJson();
                return new UploadResult((int)result.errcode,
                    (string)result.errmsg, media.Type,
                    (string)result.media_id,
                    (string)result.created_at, media);
            });
        }

        public Task<Material> DownloadMediaAsync(AgentKey key, string mediaId)
        {
            //https://qyapi.weixin.qq.com/cgi-bin/media/get?access_token=ACCESS_TOKEN&media_id=MEDIA_ID
            return Task.Run(async () =>
            {
                var token = await GetAccessTokenAsync(key);
                var result = await GetAccessDomainUrl()
                .AppendPathSegment("media")
                .AppendPathSegment("get")
                .SetQueryParams(new { access_token = token.Token, media_id = mediaId })
                .GetAsync();
                return await Material.LoadFromAsync(result.Content);
            });
        }

        public Task<Agent> GetAgentAsync(AgentKey agentKey)
        {
            //https://qyapi.weixin.qq.com/cgi-bin/agent/get?access_token=ACCESS_TOKEN&agentid=AGENTID;
            return Task.Run(async () =>
            {
                var token = await GetAccessTokenAsync(agentKey);
                var url = GetAccessDomainUrl()
                    .AppendPathSegment("agent")
                    .AppendPathSegment("get")
                    .SetQueryParams(new { access_token = token.Token, agentid = agentKey.Id });
                var result = await url.GetJsonAsync();
                return new Agent((int)result.agentid)
                {
                    Name = (string)result.name,
                    HomeUrl = (string)result.home_url,
                    SqureLogoUrl = (string)result.square_logo_url,
                    Description = (string)result.description,
                    RedirectDomain = (string)result.redirect_domain,
                    IsClosed = Convert.ToBoolean((int)result.close),
                    IsReportEnter = Convert.ToBoolean((int)result.isreportenter),
                    ReportLocationFlag = Convert.ToBoolean((int)result.report_location_flag),
                };
            });
        }
    }
}
