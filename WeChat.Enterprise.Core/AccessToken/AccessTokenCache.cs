using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System;
using Flurl.Http;
using System.Diagnostics;

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

        /// <summary>
        /// 强制更新和获取Token。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<AccessToken> UpdateAndGetTokeAsync(AgentKey key)
        {
            return Task.Run(async () =>
           {
               var token = await CreateAccessTokenCore(weChat.CorpId, key.Secret);
               cache.Set(key, token, new TimeSpan(0, 0, token.ExpirseIn));
               return token;
           });
        }

        private Task<AccessToken> CreateAccessToken(ICacheEntry entry)
        {
            return Task.Run(async () =>
            {
                var key = (AgentKey)entry.Key;
                AccessToken token = await CreateAccessTokenCore(weChat.CorpId, key.Secret);
                entry.SetValue(token).SetAbsoluteExpiration(new TimeSpan(0, 0, token.ExpirseIn));
                return token;
            });
        }

        private async Task<AccessToken> CreateAccessTokenCore(string corpId, string secret)
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = await weChat.GetAccessDomainUrl()
           .AppendPathSegment("gettoken")
           .SetQueryParams(new { corpid = corpId, corpsecret = secret })
           .GetJsonAsync();
            sw.Stop();
            if (result.errcode != 0)
            {
                throw new WeChatException((int)result.errcode, (string)result.errmsg);
            }
            AccessToken token = new AccessToken((string)result.access_token, (int)result.expires_in - (int)(sw.ElapsedMilliseconds / 1000));
            return token;
        }
    }
}
