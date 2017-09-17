using Flurl;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat.Enterprise
{
    static class Urls
    {
        private static readonly string host = "https://qyapi.weixin.qq.com/cgi-bin";
        public static Url GetAccessUrl(string corpId, string corpSecret)
        {
            return host.AppendPathSegments("gettoken").SetQueryParams(new { corpid = corpId, corpsecret = corpSecret });
        }
    }
}
