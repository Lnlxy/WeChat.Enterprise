using Flurl;
using Flurl.Http;
using System;
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
        private readonly int refreshTokenErrorCode = 42001;

        /// <summary>
        /// 获取一个值，该值标识刷新 AccessToken 的错误码。
        /// </summary>
        public int AccessTokenOverduedErrorCode => refreshTokenErrorCode;

        /// <summary>
        /// 获取或设置一个值，该值表示是否重新获取已过期的 AccessToken。
        /// </summary>
        public bool RefreshOverduedAccessToken { get; set; } = true;

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


        public TextMessageSender CreateTextSender()
        {
            return new TextMessageSender(this);
        }

        public bool NeedRefreshAccessToken(int errorCode)
        {
            return RefreshOverduedAccessToken && errorCode == refreshTokenErrorCode;
        }

        /// <summary>
        /// 获取应用访问标识。
        /// </summary>
        /// <param name="key">应用值。</param>
        /// <param name="forceUpdate">是否强制更新标识。</param>
        /// <returns></returns>
        public Task<AccessToken> GetAccessTokenAsync(AgentKey key, bool forceUpdate = false)
        {
            if (forceUpdate)
            {
                return accessTokenCache.GetAccessTokenAsync(key);
            }
            else
            {
                return accessTokenCache.UpdateAndGetTokeAsync(key);
            }

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
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<UploadResult> UploadMediaAsync(AgentKey agent, string fileName)
        {
            return Task.Run(async () =>
            {
                var token = await GetAccessTokenAsync(agent);
                var media = await Material.LoadFromAsync(fileName);
                var url = GetAccessDomainUrl()
                    .AppendPathSegment("media")
                    .AppendPathSegment("upload")
                    .SetQueryParam("type", media.Type);
                ree:
                var result = await url.SetQueryParam("access_token", token)
                    .PostAsync(media.CreateMultipartFormDataContent())
                    .ReceiveJson();
                var errorCode = (int)result.errcode;
                if (NeedRefreshAccessToken(errorCode))
                {
                    token = await GetAccessTokenAsync(agent, true);
                    goto ree;
                }
                if (errorCode != 0)
                {
                    throw new WeChatException(errorCode, (string)result.errmsg);
                }
                return new UploadResult(errorCode,
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
