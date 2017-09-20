using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System;
using Flurl.Http;

namespace WeChat.Enterprise
{
    sealed class AccessTokenCache
    {
        private readonly WeChat weChat;
        private readonly MemoryCache cache;
        private readonly object readOnlyObj = new object();
          
        public Task<AccessToken> this[AgentKey key]
        {
            get
            {
                return GetAccessTokenAsync(key);
            }
        }

        internal AccessTokenCache(WeChat weChat)
        {
            this.weChat = weChat;
            cache = new MemoryCache(new MemoryCacheOptions());
        }

        public Task<AccessToken> GetAccessTokenAsync(AgentKey key)
        {
            return cache.GetOrCreateAsync(key, CreateAccessToken);
        }

        private Task<AccessToken> CreateAccessToken(ICacheEntry arg)
        {
            return Task.Run(async () =>
            {
                var key = (AgentKey)arg.Key;
                var result = await weChat.GetAccessDomainUrl()
               .AppendPathSegment("gettoken")
               .SetQueryParams(new { corpid = weChat.CorpId, corpsecret = key.Secret })
               .GetJsonAsync();
                if (result.errcode != 0)
                {
                    throw new WeChatException((int)result.errcode, (string)result.errmsg);
                }
                AccessToken token = new AccessToken((string)result.access_token, (int)result.expires_in);
                arg.SetValue(token).SetAbsoluteExpiration(new TimeSpan(0, 0, token.ExpirseIn));
                return token;
            });
        }
    }
}
