using System;

namespace WeChat.Enterprise
{
    public struct AccessToken
    {
        /// <summary>
        /// 获取一个值，该值表示应用访问密钥。
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// 获取一个值，该值表示过期时长（秒）。
        /// </summary>
        public int ExpirseIn { get; private set; }

        /// <summary>
        /// 获取一个值，该值表示创建时间。
        /// </summary>
        public DateTime CreationDateTime { get; private set; }

        internal AccessToken(string token, int expirseIn)
        {
            CreationDateTime = DateTime.Now;
            Token = token;
            ExpirseIn = expirseIn;
        }
        public static implicit operator string(AccessToken token)
        {
            return token.Token;
        }
    }
}
