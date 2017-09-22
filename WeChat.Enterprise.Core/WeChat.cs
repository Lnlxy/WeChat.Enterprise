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
        private readonly MaterialCache materialCache;
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

        public MaterialCache MaterialCache => materialCache;

        /// <summary>
        /// 初始化 <see cref="WeChat"/> 新实例。
        /// </summary>
        /// <param name="corpId">企业Id。</param>
        public WeChat(string corpId)
        {
            CorpId = corpId;
            accessTokenCache = new AccessTokenCache(this);
            materialCache = new MaterialCache(this);
        }


        public TextMessageSender CreateTextMessageSender()
        {
            return new TextMessageSender(this);
        }

        public MaterialMessageSender CreateMaterialMessageSender()
        {
            return new MaterialMessageSender(this);
        }

        public TextCardMessageSender CreateTextCardMessageSender()
        {
            return new TextCardMessageSender(this);
        }
        public NewsMessageSender CreateNewsMessageSender()
        {
            return new NewsMessageSender(this);
        }
        public MpNewsMessageSender CreateMpNewsMessageSender()
        {
            return new MpNewsMessageSender(this);
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
                return accessTokenCache.UpdateAndGetTokeAsync(key);
            }
            else
            {
                return accessTokenCache.GetAccessTokenAsync(key);
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
